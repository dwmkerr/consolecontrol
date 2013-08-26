using System;
using System.Runtime.InteropServices;

namespace ConsoleControlAPI
{
    internal static class Imports
    {
        /// <summary>
        /// Sends a specified signal to a console process group that shares the console associated with the calling process.
        /// </summary>
        /// <param name="dwCtrlEvent">The type of signal to be generated.</param>
        /// <param name="dwProcessGroupId">The identifier of the process group to receive the signal. A process group is created when the CREATE_NEW_PROCESS_GROUP flag is specified in a call to the CreateProcess function. The process identifier of the new process is also the process group identifier of a new process group. The process group includes all processes that are descendants of the root process. Only those processes in the group that share the same console as the calling process receive the signal. In other words, if a process in the group creates a new console, that process does not receive the signal, nor do its descendants.
        /// If this parameter is zero, the signal is generated in all processes that share the console of the calling process.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("Kernel32.dll")]
        public static extern bool GenerateConsoleCtrlEvent(CTRL_EVENT dwCtrlEvent, UInt32 dwProcessGroupId);
    }

    /// <summary>
    /// The type of signal to be generated.
    /// </summary>
    internal enum CTRL_EVENT : uint
    {
        /// <summary>
        /// Generates a CTRL+C signal. This signal cannot be generated for process groups. If dwProcessGroupId is nonzero, this function will succeed, but the CTRL+C signal will not be received by processes within the specified process group.
        /// </summary>
        CTRL_C_EVENT = 0,

        /// <summary>
        /// Generates a CTRL+BREAK signal.
        /// </summary>
        CTRL_BREAK_EVENT = 1
    }
}