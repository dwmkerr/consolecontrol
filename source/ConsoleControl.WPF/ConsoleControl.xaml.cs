using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ConsoleControlAPI;

namespace ConsoleControl.WPF
{
    /// <summary>
    /// Interaction logic for ConsoleControl.xaml
    /// </summary>
    public partial class ConsoleControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleControl"/> class.
        /// </summary>
        public ConsoleControl()
        {
            InitializeComponent();
            
            //  Handle process events.
            processInterace.OnProcessOutput += processInterace_OnProcessOutput;
            processInterace.OnProcessError += processInterace_OnProcessError;
            processInterace.OnProcessInput += processInterace_OnProcessInput;
            processInterace.OnProcessExit += processInterace_OnProcessExit;

            //  Wait for key down messages on the rich text box.
            richTextBoxConsole.KeyDown += richTextBoxConsole_KeyDown;
        }

        /// <summary>
        /// Handles the OnProcessError event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessError(object sender, ProcessEventArgs args)
        {
            //  Write the output, in red
            WriteOutput(args.Content, Colors.Red);

            //  Fire the output event.
            FireProcessOutputEvent(args);
        }

        /// <summary>
        /// Handles the OnProcessOutput event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessOutput(object sender, ProcessEventArgs args)
        {
            //  Write the output, in white
            WriteOutput(args.Content, Colors.White);

            //  Fire the output event.
            FireProcessOutputEvent(args);
        }

        /// <summary>
        /// Handles the OnProcessInput event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessInput(object sender, ProcessEventArgs args)
        {
            FireProcessInputEvent(args);
        }

        /// <summary>
        /// Handles the OnProcessExit event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessExit(object sender, ProcessEventArgs args)
        {
            //  Read only again.
            RunOnUIDespatcher(() =>
                {
                    //  Are we showing diagnostics?
                    if (ShowDiagnostics)
                    {
                        WriteOutput(Environment.NewLine + processInterace.ProcessFileName + " exited.", Color.FromArgb(255, 0, 255, 0));
                    }

                    richTextBoxConsole.IsReadOnly = true;

                    //  And we're no longer running.
                    IsProcessRunning = false;
                });
        }

        /// <summary>
        /// Handles the KeyDown event of the richTextBoxConsole control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs" /> instance containing the event data.</param>
        void richTextBoxConsole_KeyDown(object sender, KeyEventArgs e)
        {
            bool inReadOnlyZone = richTextBoxConsole.Selection.Start.CompareTo(inputStartPos) < 0;

            //  If we're at the input point and it's backspace, bail.
            if (inReadOnlyZone && e.Key == Key.Back)
                e.Handled = true;

            //  Are we in the read-only zone?
            if (inReadOnlyZone)
            {
                //  Allow arrows and Ctrl-C.
                if (!(e.Key == Key.Left ||
                    e.Key == Key.Right ||
                    e.Key == Key.Up ||
                    e.Key == Key.Down ||
                    (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))))
                {
                    e.Handled = true;
                }
            }

            //  Is it the return key?
            if (e.Key == Key.Return)
            {
                //  Get the input.
                var input = new TextRange(inputStartPos, richTextBoxConsole.Selection.Start).Text;

                //  Write the input (without echoing).
                WriteInput(input, Colors.White, false);
            }
        }

        /// <summary>
        /// Writes the output to the console control.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="color">The color.</param>
        public void WriteOutput(string output, Color color)
        {
            if (string.IsNullOrEmpty(lastInput) == false &&
                (output == lastInput || output.Replace("\r\n", "") == lastInput))
                return;

            RunOnUIDespatcher(() =>
            {
                //  Write the output.
                var range = new TextRange(richTextBoxConsole.Document.ContentEnd, richTextBoxConsole.Document.ContentEnd);
                range.Text = output;
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));

                //  Record the new input start.
                inputStartPos = richTextBoxConsole.Document.ContentEnd.GetPositionAtOffset(0);
            });
        }

        /// <summary>
        /// Clears the output.
        /// </summary>
        public void ClearOutput()
        {
            richTextBoxConsole.Document.Blocks.Clear();
            inputStartPos = null;
        }

        /// <summary>
        /// Writes the input to the console control.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="color">The color.</param>
        /// <param name="echo">if set to <c>true</c> echo the input.</param>
        public void WriteInput(string input, Color color, bool echo)
        {
            RunOnUIDespatcher(() =>
                {
                    //  Are we echoing?
                    if (echo)
                    {
                        richTextBoxConsole.Selection.ApplyPropertyValue(TextBlock.ForegroundProperty, new SolidColorBrush(color));
                        richTextBoxConsole.AppendText(input);
                        inputStartPos = richTextBoxConsole.Selection.Start;
                    }

                    lastInput = input;

                    //  Write the input.
                    processInterace.WriteInput(input);

                    //  Fire the event.
                    FireProcessInputEvent(new ProcessEventArgs(input));
                });
        }

        /// <summary>
        /// Runs the on UI despatcher.
        /// </summary>
        /// <param name="action">The action.</param>
        private void RunOnUIDespatcher(Action action)
        {
            if (Dispatcher.CheckAccess())
            {
                //  Invoke the action.
                action();
            }
            else
            {
                Dispatcher.BeginInvoke(action, null);
            }
        }


        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        public void StartProcess(string fileName, string arguments)
        {
            //  Are we showing diagnostics?
            if (ShowDiagnostics)
            {
                WriteOutput("Preparing to run " + fileName, Color.FromArgb(255, 0, 255, 0));
                if (!string.IsNullOrEmpty(arguments))
                    WriteOutput(" with arguments " + arguments + "." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
                else
                    WriteOutput("." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
            }

            //  Start the process.
            processInterace.StartProcess(fileName, arguments);

            RunOnUIDespatcher(() =>
                {
                    //  If we enable input, make the control not read only.
                    if (IsInputEnabled)
                        richTextBoxConsole.IsReadOnly = false;

                    //  We're now running.
                    IsProcessRunning = true;
                    
                });
        }

        /// <summary>
        /// Stops the process.
        /// </summary>
        public void StopProcess()
        {
            //  Stop the interface.
            processInterace.StopProcess();
        }

        /// <summary>
        /// Fires the console output event.
        /// </summary>
        /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
        private void FireProcessOutputEvent(ProcessEventArgs args)
        {
            //  Get the event.
            var theEvent = OnProcessOutput;
            if (theEvent != null)
                theEvent(this, args);
        }

        /// <summary>
        /// Fires the console input event.
        /// </summary>
        /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
        private void FireProcessInputEvent(ProcessEventArgs args)
        {
            //  Get the event.
            var theEvent = OnProcessInput;
            if (theEvent != null)
                theEvent(this, args);
        }

        /// <summary>
        /// The internal process interface used to interface with the process.
        /// </summary>
        private readonly ProcessInterface processInterace = new ProcessInterface();

        /// <summary>
        /// Current position that input starts at.
        /// </summary>
        private TextPointer inputStartPos;
        
        /// <summary>
        /// The last input string (used so that we can make sure we don't echo input twice).
        /// </summary>
        private string lastInput;
        
        /// <summary>
        /// Occurs when console output is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessOutput;

        /// <summary>
        /// Occurs when console input is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessInput;
          
        private static readonly DependencyProperty ShowDiagnosticsProperty = 
          DependencyProperty.Register("ShowDiagnostics", typeof(bool), typeof(ConsoleControl),
          new PropertyMetadata(false, OnShowDiagnosticsChanged));

        /// <summary>
        /// Gets or sets a value indicating whether to show diagnostics.
        /// </summary>
        /// <value>
        ///   <c>true</c> if show diagnostics; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDiagnostics
        {
          get { return (bool)GetValue(ShowDiagnosticsProperty); }
          set { SetValue(ShowDiagnosticsProperty, value); }
        }
        
        private static void OnShowDiagnosticsChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
        }
        
        
        private static readonly DependencyProperty IsInputEnabledProperty = 
          DependencyProperty.Register("IsInputEnabled", typeof(bool), typeof(ConsoleControl),
          new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether this instance has input enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has input enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsInputEnabled
        {
          get { return (bool)GetValue(IsInputEnabledProperty); }
          set { SetValue(IsInputEnabledProperty, value); }
        }
        
        internal static readonly DependencyPropertyKey IsProcessRunningPropertyKey =
          DependencyProperty.RegisterReadOnly("IsProcessRunning", typeof(bool), typeof(ConsoleControl),
          new PropertyMetadata(false));

        private static readonly DependencyProperty IsProcessRunningProperty = IsProcessRunningPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value indicating whether this instance has a process running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a process running; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessRunning
        {
            get { return (bool)GetValue(IsProcessRunningProperty); }
            private set { SetValue(IsProcessRunningPropertyKey, value); }
        }

        /// <summary>
        /// Gets the internally used process interface.
        /// </summary>
        /// <value>
        /// The process interface.
        /// </value>
        public ProcessInterface ProcessInterface
        {
            get { return processInterace; }
        }
    }
}
