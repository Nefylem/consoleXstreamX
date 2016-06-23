using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Configuration;

namespace consoleXstreamX.DisplayMenu.MainMenu
{
    internal static class MainCommands
    {
        public static void GetMainMenuItems()
        {
            MenuSettings.Tiles.Clear();

            MenuSettings.Tiles.Add(new List<MenuSettings.MenuItems>());
            AddItem("Load Profile", "console select");
            AddItem("Save Profile", "save profile");
            AddItem("Power On", "power on");
            AddItem("", "");

            MenuSettings.Tiles.Add(new List<MenuSettings.MenuItems>());
            AddItem("Video Input", "video input");
            AddItem("Device", "video device");
            AddItem("Capture \nSettings", "video settings");
            AddItem("Display", "video display");

            MenuSettings.Tiles.Add(new List<MenuSettings.MenuItems>());
            AddItem("Output", "controller output");
            AddItem("Controller \n Settings", "output settings");
            AddItem("Remap \nInputs", "remap inputs");
            AddItem("Input \nProfile", "input profile");

            MenuSettings.Tiles.Add(new List<MenuSettings.MenuItems>());
            AddItem("System Status", "system status");
            AddItem("Configuration", "config");
            AddItem("Exit", "exit");

            MenuSettings.Selected = "video input";
        }

        private static void AddItem(string title, string command, string options = "", string active = "", bool folder = false)
        {
            var row = MenuSettings.Tiles.LastOrDefault();
            row?.Add(new MenuSettings.MenuItems()
            {
                Display = title,
                Command = command,
                DisplayOptions = options,
                ActiveFolder = active,
                Folder = folder
            });
        }
    }
}
