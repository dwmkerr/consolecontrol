using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;
using System.Windows.Input;

namespace ConsoleControlSample.WPF
{
    /// <summary>
    /// The Main ViewModel.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            StartCommandPromptCommand = new Command(() => { });
            StartNewProcessCommand = new Command(() => { });
            StopProcessCommand = new Command(() => { });
            ClearOutputCommand = new Command(() => { });
        }

        
        private NotifyingProperty ShowDiagnosticsProperty =
          new NotifyingProperty("ShowDiagnostics", typeof(bool), default(bool));

        /// <summary>
        /// Gets or sets a value indicating whether [show diagnostics].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show diagnostics]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDiagnostics
        {
            get { return (bool)GetValue(ShowDiagnosticsProperty); }
            set { SetValue(ShowDiagnosticsProperty, value); }
        }

        
        private NotifyingProperty EnableInputProperty =
          new NotifyingProperty("EnableInput", typeof(bool), default(bool));

        /// <summary>
        /// Gets or sets a value indicating whether [enable input].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable input]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableInput
        {
            get { return (bool)GetValue(EnableInputProperty); }
            set { SetValue(EnableInputProperty, value); }
        }

        
        private NotifyingProperty EnableKeyboardCommandsProperty =
          new NotifyingProperty("EnableKeyboardCommands", typeof(bool), default(bool));

        /// <summary>
        /// Gets or sets a value indicating whether [enable keyboard commands].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [enable keyboard commands]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableKeyboardCommands
        {
            get { return (bool)GetValue(EnableKeyboardCommandsProperty); }
            set { SetValue(EnableKeyboardCommandsProperty, value); }
        }
                

        /// <summary>
        /// Gets the start command prompt command.
        /// </summary>
        public Command StartCommandPromptCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the start new process command.
        /// </summary>
        /// <value>
        /// The start new process command.
        /// </value>
        public Command StartNewProcessCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the stop process command.
        /// </summary>
        /// <value>
        /// The stop process command.
        /// </value>
        public Command StopProcessCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the clear output command.
        /// </summary>
        public Command ClearOutputCommand
        {
            get;
            private set;
        }
    }
}
