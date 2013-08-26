using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConsoleControlSample.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //  Handle certain commands.
            viewModel.StartCommandPromptCommand.Executed += new Apex.MVVM.CommandEventHandler(StartCommandPromptCommand_Executed);
            viewModel.StartNewProcessCommand.Executed += new Apex.MVVM.CommandEventHandler(StartNewProcessCommand_Executed);
            viewModel.StopProcessCommand.Executed += new Apex.MVVM.CommandEventHandler(StopProcessCommand_Executed);
            viewModel.ClearOutputCommand.Executed += new Apex.MVVM.CommandEventHandler(ClearOutputCommand_Executed);
        }

        /// <summary>
        /// Handles the Executed event of the StartCommandPromptCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="Apex.MVVM.CommandEventArgs"/> instance containing the event data.</param>
        void StartCommandPromptCommand_Executed(object sender, Apex.MVVM.CommandEventArgs args)
        {
            consoleControl.StartProcess("cmd.exe", string.Empty);
        }

        /// <summary>
        /// Handles the Executed event of the StartNewProcessCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="Apex.MVVM.CommandEventArgs"/> instance containing the event data.</param>
        void StartNewProcessCommand_Executed(object sender, Apex.MVVM.CommandEventArgs args)
        {
        }

        /// <summary>
        /// Handles the Executed event of the StopProcessCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="Apex.MVVM.CommandEventArgs"/> instance containing the event data.</param>
        void StopProcessCommand_Executed(object sender, Apex.MVVM.CommandEventArgs args)
        {
        }

        /// <summary>
        /// Handles the Executed event of the ClearOutputCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="Apex.MVVM.CommandEventArgs"/> instance containing the event data.</param>
        void ClearOutputCommand_Executed(object sender, Apex.MVVM.CommandEventArgs args)
        {
        }
    }
}
