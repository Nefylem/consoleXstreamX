using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.SubMenu.Actions;

namespace consoleXstreamX.DisplayMenu.SubMenu
{
    internal static class ShutterCommand
    {
        public static int OkWait;
        private static int _leftWait;
        private static int _rightWait;
        private static int _backWait;

        public static void Execute(string command)
        {
            if (string.Equals(command, "left", StringComparison.CurrentCultureIgnoreCase)) MoveLeft();
            if (string.Equals(command, "right", StringComparison.CurrentCultureIgnoreCase)) MoveRight();
            if (string.Equals(command, "back", StringComparison.CurrentCultureIgnoreCase)) StepBackMenu();
            if (string.Equals(command, "ok", StringComparison.CurrentCultureIgnoreCase)) MenuOk();
        }

        private static void MoveLeft()
        {
            if (_leftWait > 0) return;
            _leftWait = MenuCommand.SetMoveWait();
            var index = Shutter.Tiles.FindIndex(s => string.Equals(s.Command, Shutter.Selected, StringComparison.CurrentCultureIgnoreCase));
            if (index <= 0) return;
            index--;
            Shutter.Selected = Shutter.Tiles[index].Command;
        }

        private static void MoveRight()
        {
            if (_rightWait > 0) return;
            _rightWait = MenuCommand.SetMoveWait();

            var index = Shutter.Tiles.FindIndex(s => string.Equals(s.Command, Shutter.Selected, StringComparison.CurrentCultureIgnoreCase));
            if (index >= Shutter.Tiles.Count - 1) return;
            index++;
            Shutter.Selected = Shutter.Tiles[index].Command;
        }

        public static void StepBackMenu()
        {
            if (_backWait > 0) return;
            if (MenuSettings.History.Count > 0) MenuSettings.History.RemoveAt(MenuSettings.History.Count - 1);
            if (MenuSettings.History.Count == 0)
            {
                MenuCommand.OkWait = 5;
                Shutter.Hide = true;
            }
            else
            {
                //Step back one menu
            }
        }

        private static void MenuOk()
        {
            if (OkWait > 0)
            {
                OkWait++;
                return;
            }
            OkWait = MenuCommand.SetMoveWait()*3;
            if (string.Equals(MenuSettings.CurrentMenu, "Exit", StringComparison.CurrentCultureIgnoreCase)) SelectExit.Execute(Shutter.Selected);
        }

        public static void CheckDelays()
        {
            if (_leftWait > 0) _leftWait--;
            if (_rightWait > 0) _rightWait--;
            if (_backWait > 0) _backWait--;
            if (OkWait > 0) OkWait--;
        }
    }
}
