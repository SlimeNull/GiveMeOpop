using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GiveMeOpop
{
    internal static class NativeMethods
    {
        [DllImport("ntdll.dll", SetLastError = false)]
        public static extern uint RtlSetProcessIsCritical(bool bNew, IntPtr expectsZero, bool bNeedsScb);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        const int ProcessBreakOnTermination = 0x1D;

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref uint processInformation, int processInformationLength);

        public static bool DontStopProcess(Process process)
        {
            uint breakOnTerminationValue = 1;  // nonzero value will enable the ProcessBreakOnTermination
                                               // Assume you know the PID of the process to set as critical

            int status = NtSetInformationProcess(process.Handle, ProcessBreakOnTermination, ref breakOnTerminationValue, Marshal.SizeOf(typeof(uint)));
            if (status < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
