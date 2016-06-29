using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Input;

namespace consoleXstreamX.Output
{
    internal static class CmPlus
    {
        public static void Send(Gamepad.GamepadOutput player)
        {
            if (CronusmaxPlus.Write == null) return;
            if (MenuController.Visible) return;
            if (CronusmaxPlus.Connected() != 1) return;

            CronusmaxPlus.Write(player.Output);
            var report = new CronusmaxPlus.Report();
            if (CronusmaxPlus.Read(ref report) == IntPtr.Zero) return;

        }
    }
}
