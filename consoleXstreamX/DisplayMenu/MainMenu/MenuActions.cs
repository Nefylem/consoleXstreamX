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
            if (string.Equals(command, "video input", StringComparison.CurrentCultureIgnoreCase)) SelectVideoInput.Show();
            if (string.Equals(command, "video device", StringComparison.CurrentCultureIgnoreCase)) SelectVideoDevice.Show();

            if (string.Equals(command, "exit", StringComparison.CurrentCultureIgnoreCase))
            {
                SelectExit.Show();
                return;
            }
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
            Shutter.Scroll = 0;
            Shutter.Error = "";
            Shutter.Explanation = "";
        }
    }
}
