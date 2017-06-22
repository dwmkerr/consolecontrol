using System;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace ConsoleControlAPI
{
    /// <summary>
    /// A ProcessEventHandler is a delegate for process input/output events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
    public delegate void ProcessEventHanlder(object sender, ProcessEventArgs args);

    /// <summary>
    /// A class the wraps a process, allowing programmatic input and output.
    /// </summary>
    /// <remarks>
    /// Changes:
    /// 1. IDisposable code contributions are from @webmaster442, on github.
    /// 2. Base working directory was changed by @fantoms, on github.
    /// </remarks>
    public class ProcessInterface : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInterface"/> class.
        /// </summary>
        public ProcessInterface()
        {
            //  Configure the output worker.
            outputWorker.WorkerReportsProgress = true;
            outputWorker.WorkerSupportsCancellation = true;
            outputWorker.DoWork += outputWorker_DoWork;
            outputWorker.ProgressChanged += outputWorker_ProgressChanged;

            //  Configure the error worker.
            errorWorker.WorkerReportsProgress = true;
            errorWorker.WorkerSupportsCancellation = true;
            errorWorker.DoWork += errorWorker_DoWork;
            errorWorker.ProgressChanged += errorWorker_ProgressChanged;
        }

        /// <summary>
        /// Handles the ProgressChanged event of the outputWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        void outputWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  We must be passed a string in the user state.
            if (e.UserState is string)
            {
                //  Fire the output event.
                FireProcessOutputEvent(e.UserState as string);
            }
        }

        /// <summary>
        /// Handles the DoWork event of the outputWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        void outputWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (outputWorker.CancellationPending == false)
            {
                //  Any lines to read?
                int count;
                var buffer = new char[1024];
                do
                {
                    var builder = new StringBuilder();
                    count = outputReader.Read(buffer, 0, 1024);
                    builder.Append(buffer, 0, count);
                    outputWorker.ReportProgress(0, builder.ToString());
                } while (count > 0);

                System.Threading.Thread.Sleep(200);
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of the errorWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        void errorWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  The userstate must be a string.
            if (e.UserState is string)
            {
                //  Fire the error event.
                FireProcessErrorEvent(e.UserState as string);
            }
        }

        /// <summary>
        /// Handles the DoWork event of the errorWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        void errorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (errorWorker.CancellationPending == false)
            {
                //  Any lines to read?
                int count;
                var buffer = new char[1024];
                do
                {
                    var builder = new StringBuilder();
                    count = errorReader.Read(buffer, 0, 1024);
                    builder.Append(buffer, 0, count);
                    errorWorker.ReportProgress(0, builder.ToString());
                } while (count > 0);

                System.Threading.Thread.Sleep(200);
            }
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        public void StartProcess(string fileName, string arguments)
        {
            //  Create the process start info.
            var processStartInfo = new ProcessStartInfo(fileName, arguments);

            //  Set the options.
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;

            //  Specify redirection.
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            //  Create the process.
            process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;
            process.Exited += currentProcess_Exited;

            //  Start the process.
            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                //  Trace the exception.
                Trace.WriteLine("Failed to start process " + fileName + " with arguments '" + arguments + "'");
                Trace.WriteLine(e.ToString());
                return;
            }

            //  Store name and arguments.
            processFileName = fileName;
            processArguments = arguments;

            //  Create the readers and writers.
            inputWriter = process.StandardInput;
            outputReader = TextReader.Synchronized(process.StandardOutput);
            errorReader = TextReader.Synchronized(process.StandardError);

            //  Run the workers that read output and error.
            outputWorker.RunWorkerAsync();
            errorWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="workingDirectory">Starting folder.</param>
        public void StartProcess(string fileName, string arguments, string workingDirectory)
        {
            //  Create the process start info.
            var processStartInfo = new ProcessStartInfo(fileName, arguments);

            //  Set the options.
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WorkingDirectory = workingDirectory;

            //  Specify redirection.
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            //  Create the process.
            process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;
            process.Exited += currentProcess_Exited;

            //  Start the process.
            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                var msg = $"Failed to start process {fileName} with arguments '{arguments}'";
                //  Trace the exception.
                Trace.WriteLine(msg);
                Trace.WriteLine(e.ToString());
                FireProcessErrorEvent(msg);
                return;
            }

            //  Store name and arguments.
            processFileName = fileName;
            processArguments = arguments;

            //  Create the readers and writers.
            inputWriter = process.StandardInput;
            outputReader = TextReader.Synchronized(process.StandardOutput);
            errorReader = TextReader.Synchronized(process.StandardError);

            //  Run the workers that read output and error.
            outputWorker.RunWorkerAsync();
            errorWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="workingDirectory">Starting folder.</param>
        public string StartProcessResult(string fileName, string arguments, string workingDirectory)
        {
            //  Create the process start info.
            var processStartInfo = new ProcessStartInfo(fileName, arguments);

            //  Set the options.
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WorkingDirectory = workingDirectory;

            //  Specify redirection.
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            //  Create the process.
            process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;
            process.Exited += currentProcess_Exited;

            //  Start the process.
            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                var msg = $"Failed to start process {fileName} with arguments '{arguments}'";
                process = null;
                //  Trace the exception.
                Trace.WriteLine(msg);
                Trace.WriteLine(e.ToString());
                return msg;
            }

            //  Store name and arguments.
            processFileName = fileName;
            processArguments = arguments;

            //  Create the readers and writers.
            inputWriter = process.StandardInput;
            outputReader = TextReader.Synchronized(process.StandardOutput);
            errorReader = TextReader.Synchronized(process.StandardError);

            //  Run the workers that read output and error.
            outputWorker.RunWorkerAsync();
            errorWorker.RunWorkerAsync();
            return string.Empty;
        }

        /// <summary>
        /// Stops the process.
        /// </summary>
        public void StopProcess()
        {
            //  Handle the trivial case.
            if (IsProcessRunning == false)
                return;

            //  Kill the process.
            process.Kill();
        }

        /// <summary>
        /// Handles the Exited event of the currentProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void currentProcess_Exited(object sender, EventArgs e)
        {
            //  Fire process exited.
            FireProcessExitEvent(process.ExitCode);

            //  Disable the threads.
            outputWorker.CancelAsync();
            errorWorker.CancelAsync();
            inputWriter = null;
            outputReader = null;
            errorReader = null;
            process = null;
            processFileName = null;
            processArguments = null;
        }

        /// <summary>
        /// Fires the process output event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireProcessOutputEvent(string content)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessOutput;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(content));
        }

        /// <summary>
        /// Fires the process error output event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireProcessErrorEvent(string content)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessError;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(content));
        }

        /// <summary>
        /// Fires the process input event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireProcessInputEvent(string content)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessInput;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(content));
        }

        /// <summary>
        /// Fires the process exit event.
        /// </summary>
        /// <param name="code">The code.</param>
        private void FireProcessExitEvent(int code)
        {
            //  Get the event and fire it.
            var theEvent = OnProcessExit;
            if (theEvent != null)
                theEvent(this, new ProcessEventArgs(code));
        }

        /// <summary>
        /// Writes the input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void WriteInput(string input)
        {
            if (IsProcessRunning)
            {
                inputWriter.WriteLine(input);
                inputWriter.Flush();
            }
        }

        /// <summary>
        /// Disposes of the Process Interface
        /// </summary>
        ~ProcessInterface()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose function to unload process resources after checking if the internal objects have been disposed.
        /// </summary>
        protected void Dispose(bool native)
        {
            if (outputWorker != null)
            {
                outputWorker.Dispose();
                outputWorker = null;
            }
            if (errorWorker != null)
            {
                errorWorker.Dispose();
                errorWorker = null;
            }
            if (process != null)
            {
                process.Dispose();
                process = null;
            }
            if (inputWriter != null)
            {
                inputWriter.Dispose();
                inputWriter = null;
            }
            if (outputReader != null)
            {
                outputReader.Dispose();
                outputReader = null;
            }
            if (errorReader != null)
            {
                errorReader.Dispose();
                errorReader = null;
            }
        }

        /// <summary>
        /// Main Dispose function.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The current process.
        /// </summary>
        private Process process;
        
        /// <summary>
        /// The input writer.
        /// </summary>
        private StreamWriter inputWriter;
        
        /// <summary>
        /// The output reader.
        /// </summary>
        private TextReader outputReader;
        
        /// <summary>
        /// The error reader.
        /// </summary>
        private TextReader errorReader;
        
        /// <summary>
        /// The output worker.
        /// </summary>
        private BackgroundWorker outputWorker = new BackgroundWorker();
        
        /// <summary>
        /// The error worker.
        /// </summary>
        private BackgroundWorker errorWorker = new BackgroundWorker();

        /// <summary>
        /// Current process file name.
        /// </summary>
        private string processFileName;

        /// <summary>
        /// Arguments sent to the current process.
        /// </summary>
        private string processArguments;
        
        /// <summary>
        /// Occurs when process output is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessOutput;

        /// <summary>
        /// Occurs when process error output is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessError;

        /// <summary>
        /// Occurs when process input is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessInput;

        /// <summary>
        /// Occurs when the process ends.
        /// </summary>
        public event ProcessEventHanlder OnProcessExit;
        
        /// <summary>
        /// Gets a value indicating whether this instance is process running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is process running; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessRunning
        {
            get
            {
                try
                {
                    return (process != null && process.HasExited == false);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the internal process.
        /// </summary>
        public Process Process
        {
            get { return process; }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        /// <value>
        /// The name of the process.
        /// </value>
        public string ProcessFileName
        {
            get { return processFileName; }
        }

        /// <summary>
        /// Gets the process arguments.
        /// </summary>
        public string ProcessArguments
        {
            get { return processArguments; }
        }
    }
}
