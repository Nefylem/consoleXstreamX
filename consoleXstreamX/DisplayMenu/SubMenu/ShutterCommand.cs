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
            if (index - Shutter.Scroll <= 1)
            {
                if (Shutter.Scroll > 0) Shutter.Scroll--;
            }
            index--;
            Shutter.Selected = Shutter.Tiles[index].Command;
        }

        private static void MoveRight()
        {
            if (_rightWait > 0) return;
            _rightWait = MenuCommand.SetMoveWait();

            var index = Shutter.Tiles.FindIndex(s => string.Equals(s.Command, Shutter.Selected, StringComparison.CurrentCultureIgnoreCase));
            if (index >= Shutter.Tiles.Count - 1) return;
            var b = Shutter.Scroll;
            index++;
            if (index - Shutter.Scroll >= 3)
            {
                if (Shutter.Scroll + index < Shutter.Tiles.Count) Shutter.Scroll++;
            }
            Shutter.Selected = Shutter.Tiles[index].Command;
        }

        public static void StepBackMenu()
        {
            if (MenuSettings.CalibrateController)
            {
                //Calculate feed ratios
                MenuSettings.CalibrateController = false;
                CalibrateController.Calculate();
            }
            if (_backWait > 0) return;
            if (MenuSettings.History.Count > 0) MenuSettings.History.RemoveAt(MenuSettings.History.Count - 1);
            if (MenuSettings.History.Count == 0)
            {
                MenuCommand.OkWait = 10;
                MenuCommand.BackWait = 10;
                Shutter.Hide = true;
            }
            else
            {
                //Step back one menu
            }
        }

        private static void MenuOk()
        {
            if (OkWait > 0) return;
            OkWait = MenuCommand.SetMoveWait()*3;
            if (string.Equals(MenuSettings.CurrentMenu, "Save Profile", StringComparison.CurrentCultureIgnoreCase)) SelectSaveProfile.Save(Shutter.Selected);
            if (string.Equals(MenuSettings.CurrentMenu, "Load Profile", StringComparison.CurrentCultureIgnoreCase)) SelectLoadProfile.Load(Shutter.Selected);

            if (string.Equals(MenuSettings.CurrentMenu, "TitanOne Output", StringComparison.CurrentCultureIgnoreCase)) SelectControllerOutput.SelectTitanOne(Shutter.Selected);
            if (string.Equals(MenuSettings.CurrentMenu, "Controller Output", StringComparison.CurrentCultureIgnoreCase)) SelectControllerOutput.Select(Shutter.Selected);
            if (string.Equals(MenuSettings.CurrentMenu, "Controller Settings", StringComparison.CurrentCultureIgnoreCase)) SelectControllerSettings.Execute(Shutter.Selected);
            if (string.Equals(MenuSettings.CurrentMenu, "Video Input", StringComparison.CurrentCultureIgnoreCase)) SelectVideoInput.Execute(Shutter.Selected);
            if (string.Equals(MenuSettings.CurrentMenu, "Video Device", StringComparison.CurrentCultureIgnoreCase)) SelectVideoDevice.Execute(Shutter.Selected);
            if (string.Equals(MenuSettings.CurrentMenu, "Video Display", StringComparison.CurrentCultureIgnoreCase)) SelectVideoDisplay.Execute(Shutter.Selected);
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
