using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GiveMeOpop
{
    internal static class NativeMethods
    {
        [DllImport("ntdll.dll", SetLastError = false)]
        public static extern uint RtlSetProcessIsCritical(bool bNew, IntPtr expectsZero, bool bNeedsScb);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);
    }
}
