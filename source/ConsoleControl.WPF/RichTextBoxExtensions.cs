using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

//  Notes: Many thanks to https://github.com/adamecr/RadProjectsExtension for this code!
//  https://github.com/dwmkerr/consolecontrol/issues/25#issuecomment-437586440

namespace ConsoleControl.WPF
{
    internal static class RichTextBoxExtensions
    {
        /// <summary>
        /// Gets the current caret position (offset from start)
        /// </summary>
        /// <param name="richTextBox">Rich text box to work with</param>
        /// <returns>The current caret position (offset from start)</returns>
        public static int GetCaretPosition(this RichTextBox richTextBox)
        {
            return richTextBox.Document.ContentStart.GetOffsetToPosition(richTextBox.CaretPosition);
        }

        /// <summary>
        /// Gets the position of the end of the content (offset from start)
        /// </summary>
        /// <param name="richTextBox">Rich text box to work with</param>
        /// <returns>The position of the end of the content (offset from start)</returns>
        public static int GetEndPosition(this RichTextBox richTextBox)
        {
            return richTextBox.Document.ContentStart.GetOffsetToPosition(richTextBox.Document.ContentEnd);
        }

        /// <summary>
        /// Gets the pointer to the end of the content
        /// </summary>
        /// <param name="richTextBox">Rich text box to work with</param>
        /// <returns>The pointer to the end of the content</returns>
        public static TextPointer GetEndPointer(this RichTextBox richTextBox)
        {
            return richTextBox.Document.ContentEnd;
        }

        /// <summary>
        ///  Gets the pointer to given <paramref name="position"/> (offset from start) within the content
        /// </summary>
        /// <param name="richTextBox">Rich text box to work with</param>
        /// <param name="position">Offset from start</param>
        /// <returns>The pointer at given position</returns>
        public static TextPointer GetPointerAt(this RichTextBox richTextBox, int position)
        {
            return richTextBox.Document.ContentStart.GetPositionAtOffset(position);
        }

        /// <summary>
        /// Sets the caret to the end of the content
        /// </summary>
        /// <param name="richTextBox">Rich text box to work with</param>
        public static void SetCaretToEnd(this RichTextBox richTextBox)
        {
        richTextBox.CaretPosition = richTextBox.GetEndPointer();
        }
    }
}
