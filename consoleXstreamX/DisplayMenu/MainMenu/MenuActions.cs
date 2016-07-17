using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.SubMenu;
using consoleXstreamX.DisplayMenu.SubMenu.Actions;

namespace consoleXstreamX.DisplayMenu.MainMenu
{
    internal static class MenuActions
    {
        public static void Run(string command)
        {
            if (string.Equals(command, "load profile", StringComparison.CurrentCultureIgnoreCase)) SelectLoadProfile.Show();
            if (string.Equals(command, "save profile", StringComparison.CurrentCultureIgnoreCase)) SelectSaveProfile.Show();
            if (string.Equals(command, "power on", StringComparison.CurrentCultureIgnoreCase)) SelectPowerOn.Show();
            if (string.Equals(command, "video input", StringComparison.CurrentCultureIgnoreCase)) SelectVideoInput.Show();
            if (string.Equals(command, "video device", StringComparison.CurrentCultureIgnoreCase)) SelectVideoDevice.Show();
            if (string.Equals(command, "video display", StringComparison.CurrentCultureIgnoreCase)) SelectVideoDisplay.Show();
            if (string.Equals(command, "controller output", StringComparison.CurrentCultureIgnoreCase)) SelectControllerOutput.Show();
            if (string.Equals(command, "Controller Settings", StringComparison.CurrentCultureIgnoreCase)) SelectControllerSettings.Show();
            if (string.Equals(command, "exit", StringComparison.CurrentCultureIgnoreCase)) SelectExit.Show();
        }

        public static void SetMenu(string command)
        {
            MenuSettings.CurrentMenu = command;
            MenuSettings.History.Add(command);
            ShutterCommand.OkWait = MenuCommand.SetMoveWait()*3;
        }

        public static void ClearSubMenu()
        {
            Shutter.Tiles.Clear();
            Shutter.CheckedItems.Clear();
            Shutter.Scroll = 0;
            Shutter.Error = "";
            Shutter.Explanation = "";
        }
    }
}
