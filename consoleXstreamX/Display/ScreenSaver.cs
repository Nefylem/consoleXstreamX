using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace consoleXstreamX.Display
{
    internal static class ScreenSaver
    {
        [DllImport("kernel32.dll")]
        static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [FlagsAttribute]
        enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
        }

        public static void DisableScreenSaver()
        {
            SetThreadExecutionState(ExecutionState.EsDisplayRequired | ExecutionState.EsContinuous);
        }

        public static void EnableScreenSaver()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous);
        }
    }
}
