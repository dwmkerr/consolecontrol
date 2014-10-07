///**************************************************************************************||///
/// DISCLAIMER:                                                                          ||\\\
/// The code in this file IS NOT original work from Dave Kerr, but its use is free.      ||///
/// It was based on:                                                                     ||\\\
/// http://www.codeproject.com/Articles/9196/Links-with-arbitrary-text-in-a-RichTextBox  ||///
/// The most recent version of this code can always be found at:                         ||\\\
/// https://github.com/israelss/AnyLinkRichTextBox                                       ||///
///**************************************************************************************||\\\

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AnyLinkRichTextBox
{
    public partial class AnyLinkRichTextBox : RichTextBox
   {
		#region Interop-Defines
		#region CHARFORMAT2 Struct & Flags
        private CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();

		[ StructLayout( LayoutKind.Sequential )]
		private struct CHARFORMAT2_STRUCT
		{
			public UInt32	cbSize; 
			public UInt32   dwMask; 
			public UInt32   dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily; 
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
			public char[]   szFaceName; 
			public UInt16	wWeight;
			public UInt16	sSpacing;
			public int		crBackColor; // Color.ToArgb() -> int
			public int		lcid;
			public int		dwReserved;
			public Int16	sStyle;
			public Int16	wKerning;
			public byte		bUnderlineType;
			public byte		bAnimation;
			public byte		bRevAuthor;
			public byte		bReserved1;
		}
        
        //Flags:
		private const UInt32 CFE_LINK		= 0x0020;
		private const UInt32 CFM_LINK		= 0x00000020;
		#endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_USER = 0x0400;
        private const int EM_SETCHARFORMAT = WM_USER + 68;
        private const int SCF_SELECTION = 0x0001;
		#endregion

        #region "Pause drawing functions"
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        
        private const Int32 WM_SETREDRAW = 0xB;
        private const int FALSE = 0;
        private const int TRUE = 1;

        /// <summary>
        /// That method is used to pause the drawing of the rich text box, so the user doesn't see
        /// any flickering nor the process of parsing the text looking for links
        /// </summary>
        private void SuspendDrawing()
        {
            SendMessage(this.Handle, WM_SETREDRAW, FALSE, 0);
        }

        /// <summary>
        /// That method is used to resume the drawing, so the user can see any modification
        /// on the text, as a result of the process of parsing the text looking for links
        /// </summary>
        private void ResumeDrawing()
        {
            SendMessage(this.Handle, WM_SETREDRAW, TRUE, 0);
            this.Invalidate();
        }
        #endregion

        #region Regex
        /// <summary>
        /// This Regex is used for parse the text and search for any custom link in the Markdown style,
        /// in the form '[Friendly Text](Hyperlink Text)'
        /// </summary>
        private static Regex customLinks = new Regex(
            @"\[.*\S.*\]\(.*\S.*\)",
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant |
            RegexOptions.Compiled);

        /// <summary>
        /// This Regex is used for parse the text and search for any normal link,
        /// by normal, I mean any link which starts with any protocol (http://|https://|etc...)
        /// or without protocol, but starting with 'www.' (www.example.com)
        /// </summary>
        private static Regex normalLinks = new Regex(
            @"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*|(?<Domain>w{3}\.[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*",
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant |
            RegexOptions.Compiled);

        /// <summary>
        /// This Regex is used for parse the text and search for any IP like link,
        /// for example '255.255.255.255'
        /// </summary>
        private static Regex IPLinks = new Regex(
            @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)",
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant |
            RegexOptions.Compiled);

        /// <summary>
        /// This Regex is used for parse the text and search for any mail like link,
        /// for example 'user@company.com'.
        /// The mail links which starts with the protocol 'mailto:' are identified
        /// with the Regex normalLinks
        /// </summary>
        private static Regex mailLinks = new Regex(
            @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})",
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant |
            RegexOptions.Compiled);
        #endregion

        #region Variables
        private Dictionary<KeyValuePair<int, int>, string> hyperlinks = new Dictionary<KeyValuePair<int, int>, string>();
        private Point pt;
        private char[] spliters;
        private int OldLength;        
        #endregion

        #region Constructor
        public AnyLinkRichTextBox()
        {
            // Otherwise, non-standard links get lost when user starts typing
            // next to a non-standard link
            base.DetectUrls = false;
            this.DetectUrls = true;
            spliters = new char[] { '[', ']', '(', ')' };
            this.LinkClicked += RTBExCustomLinks_LinkClicked;
            this.TextChanged += RTBExCustomLinks_TextChanged;
            this.MouseMove += RTBExCustomLinks_MouseMove;
            this.Protected += RTBExCustomLinks_Protected;
        }
        #endregion

        #region Properties
        /// <summary>
        /// That property overrides base.DetectUrls
        /// </summary>
        [Browsable(true),
        DefaultValue(false)]
        public new bool DetectUrls { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// This event occurs when the user tries to modify any protected text.
        /// So it is used to verify when the user want to modify a custom link,
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The args of event</param>
        private void RTBExCustomLinks_Protected(object sender, EventArgs e)
        {
            if (DetectUrls)
            {
                int i = 0;
                bool protectedBegin = true;
                bool protectedEnd = true;
                KeyValuePair<int, int> key;
                while (protectedBegin)
                {
                    i++;
                    int previous = this.SelectionStart - 1;
                    this.Select(previous - 1, i);
                    protectedBegin = this.SelectionProtected;
                    this.Select(previous, i);
                }
                if (!(this.SelectionStart + this.SelectionLength == this.Text.Length))
                {
                    while (protectedEnd && !(this.SelectionStart + this.SelectionLength == this.Text.Length))
                    {
                        i++;
                        this.Select(this.SelectionStart, i);
                        protectedEnd = this.SelectionProtected;
                        this.Select(this.SelectionStart, i - 1);
                    }
                }
                string text = this.SelectedText;
                this.SelectionProtected = false;
                key = new KeyValuePair<int, int>(this.SelectionStart, text.Length);
                this.SelectedText = String.Concat(spliters[0], text, spliters[1], spliters[2], hyperlinks[key]);
                hyperlinks.Remove(key);
            }
        }

        /// <summary>
        /// This event occurs whenever the mouse move in the control area,
        /// so it is used to know where the user clicked and later calculate
        /// the index of caret at that position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RTBExCustomLinks_MouseMove(object sender, MouseEventArgs e)
        {
            pt = e.Location;
        }

        /// <summary>
        /// This event occurs whenever the rich text box text is changed,
        /// so it is used to start the parsing of text, to search for any changes
        /// in links, adding or removing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RTBExCustomLinks_TextChanged(object sender, EventArgs e)
        {
            if (DetectUrls)
            {
                SuspendDrawing();
                int pos = this.SelectionStart;

                pos = CheckCustomLinks(pos);
                MoveCustomLinks();
                RemoveLinks();

                CheckNormalLinks();
                CheckMailLinks();
                CheckIPLinks();

                RefreshCustomLinks();

                if (pos > 0) this.Select(pos, 0);
                else this.Select(0, 0);
                ResumeDrawing();
            }
        }

        /// <summary>
        /// This event occurs whenever the user clicks a link,
        /// so it is used to process the link according to the type
        /// (normal, IP, mail or custom link)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RTBExCustomLinks_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (DetectUrls)
            {
                if (normalLinks.IsMatch(e.LinkText))
                {
                    Process.Start(e.LinkText);
                }
                else if (mailLinks.IsMatch(e.LinkText))
                {
                    Process.Start("mailto:" + e.LinkText);
                }
                else if (IPLinks.IsMatch(e.LinkText))
                {
                    Process.Start("http://" + e.LinkText);
                }
                else
                {
                    int mouseClick = this.GetCharIndexFromPosition(pt);
                    try
                    {
                        var linkClicked = hyperlinks.Where(k => IsInRange(mouseClick, k.Key.Key, k.Key.Value));
                        string hyperlinkClicked = linkClicked.Select(k => k.Value).ToList().First();
                        this.SelectionStart = linkClicked.Select(k => k.Key.Key).First() + linkClicked.Select(k => k.Key.Value).First();
                        Process.Start(hyperlinkClicked);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The link is not valid!");
                    }
                }
            }
        }
        #endregion

        #region TextChanged Methods
        //These are the methods used for parsing the text.
        //They are fired in that order:
        private int CheckCustomLinks(int pos)
        {
            if (customLinks.Matches(this.Text).Cast<Match>().Any())
            {
                var linksCustom = customLinks.Matches(this.Text).Cast<Match>().Select(n => n).ToList();
                foreach (var item in linksCustom)
                {
                    var parsedLink = item.Value.Split(spliters, StringSplitOptions.RemoveEmptyEntries);
                    string text = parsedLink[0];
                    string hyperlink = parsedLink[1];
                    int start = item.Index;
                    int length = item.Length;
                    KeyValuePair<int, int> key = new KeyValuePair<int, int>(start, text.Length);
                    if (hyperlinks.ContainsKey(key) || hyperlinks.Keys.Any(k => k.Key == key.Key))
                    {
                        hyperlinks.Remove(key);
                        hyperlinks.Add(key, hyperlink);
                    }
                    else
                    {
                        hyperlinks.Add(key, hyperlink);
                    };

                    this.SelectionStart = start;
                    this.Select(start, length);
                    this.SelectedText = text;
                    this.Select(start, text.Length);
                    this.SetSelectionStyle(CFM_LINK, CFE_LINK);
                    this.SelectionProtected = true;
                    int pos2 = (pos - length) + text.Length;
                    if (pos2 > 0)
                    {
                        this.Select(pos2, 0);
                        pos = pos2;
                    }
                    else this.Select(0, 0);
                }
            }
            return pos;
        }

        private void MoveCustomLinks()
        {
            int lengthDiff = 0;
            if (OldLength != 0)
            {
                lengthDiff = this.Text.Length - OldLength;
            }

            if (hyperlinks.Any() && lengthDiff != 0)
            {
                var keysToUpdate = new List<KeyValuePair<int, int>>();

                foreach (var entry in hyperlinks)
                {
                    keysToUpdate.Add(entry.Key);
                }

                foreach (var keyToUpdate in keysToUpdate)
                {
                    var value = hyperlinks[keyToUpdate];
                    int newKey;
                    if (this.SelectionStart <= keyToUpdate.Key + lengthDiff)
                    {
                        newKey = keyToUpdate.Key + lengthDiff;
                    }
                    else
                    {
                        newKey = keyToUpdate.Key;
                    }

                    hyperlinks.Remove(keyToUpdate);
                    hyperlinks.Add(new KeyValuePair<int, int>(newKey, keyToUpdate.Value), value);
                }
            }
            OldLength = this.Text.Length;
        }

        private void RemoveLinks()
        {
            this.SelectAll();
            this.SelectionProtected = false;
            this.SetSelectionStyle(CFM_LINK, 0);
        }

        private void CheckNormalLinks()
        {
            if (normalLinks.Matches(this.Text).Cast<Match>().Any())
            {
                var linksNormal = normalLinks.Matches(this.Text).Cast<Match>().Select(n => n).ToList();
                foreach (var item in linksNormal)
                {
                    this.Select(item.Index, item.Length);
                    this.SetSelectionStyle(CFM_LINK, CFE_LINK);
                }
            }
        }

        private void CheckMailLinks()
        {
            if (mailLinks.Matches(this.Text).Cast<Match>().Any())
            {
                var linksMail = mailLinks.Matches(this.Text).Cast<Match>().Select(n => n).ToList();
                foreach (var item in linksMail)
                {
                    this.Select(item.Index, item.Length);
                    this.SetSelectionStyle(CFM_LINK, CFE_LINK);
                }
            }
        }

        private void CheckIPLinks()
        {
            if (IPLinks.Matches(this.Text).Cast<Match>().Any())
            {
                var linksIP = IPLinks.Matches(this.Text).Cast<Match>().Select(n => n).ToList();
                foreach (var item in linksIP)
                {
                    this.Select(item.Index, item.Length);
                    this.SetSelectionStyle(CFM_LINK, CFE_LINK);
                }
            }
        }

        private void RefreshCustomLinks()
        {
            foreach (var item in hyperlinks.Keys)
            {
                this.Select(item.Key, item.Value);
                this.SetSelectionStyle(CFM_LINK, CFE_LINK);
                this.SelectionProtected = true;
            }
        }        
        #endregion

        #region Misc
        /// <summary>
        /// This method is used to know which link the user has clicked
        /// </summary>
        /// <param name="mouseClick">The caret index at the position where the user has clicked</param>
        /// <param name="start">The start index of link</param>
        /// <param name="length">The length of link</param>
        /// <returns>Returns true if the user has clicked that link, false otherwise</returns>
        private bool IsInRange(int mouseClick, int start, int length)
        {
            int end = start + length;
            if (mouseClick >= start && mouseClick <= end) return true;
            else return false;
        }

        /// <summary>
        /// Set the current selection's link style
        /// </summary>
        private void SetSelectionStyle(UInt32 mask, UInt32 effect)
		{
			cf.cbSize = (UInt32)Marshal.SizeOf(cf);
			cf.dwMask = mask;
			cf.dwEffects = effect;

			IntPtr wpar = new IntPtr(SCF_SELECTION);
			IntPtr lpar = Marshal.AllocCoTaskMem( Marshal.SizeOf( cf ) ); 
			Marshal.StructureToPtr(cf, lpar, false);

			IntPtr res = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);

			Marshal.FreeCoTaskMem(lpar);
		}        
        #endregion
	}
}
