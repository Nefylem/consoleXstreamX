﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Configuration;
using consoleXstreamX.Debugging;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Input;

namespace consoleXstreamX.Output
{
    internal static class CmPlus
    {
        private static bool _logNotConnected;
        private static bool _logConnected;

        public static void Send(Gamepad.GamepadOutput player)
        {
            if (CronusmaxPlus.Write == null) return;
            if (MenuController.Visible) return;
            if (CronusmaxPlus.Connected() != 1)
            {
                if (!_logNotConnected)
                {
                    _logConnected = false;
                    _logNotConnected = true;
                    Debug.Log("CMPlus not connected");
                }
                return;
            }

            if (!_logConnected)
            {
                _logConnected = true;
                _logNotConnected = false;
                Debug.Log("CMPlus connected");
            }

            CronusmaxPlus.Write(player.Output);
            var report = new CronusmaxPlus.Report();
            if (CronusmaxPlus.Read(ref report) == IntPtr.Zero) return;
            if (Settings.Rumble) Gamepad.SetState(player.Index, report.Rumble[0], report.Rumble[1]);
        }
    }
}
