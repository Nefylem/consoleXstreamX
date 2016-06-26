using System;
using System.Linq;
using consoleXstreamX.Capture;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectVideoDisplay
    {
        public static void Show()
        {
            MenuActions.SetMenu("Video Display");
            MenuActions.ClearSubMenu();

            var type = "Bounds";
            if (Settings.Fullscreen) type = "WindowState";

            Shutter.AddItem("Fullscreen", "fullscreen", type);
            Shutter.Selected = "fullscreen";
            Shutter.SetActiveRow(1);
        }

        public static void Execute(string command)
        {
            if (string.Equals(command, "fullscreen", StringComparison.CurrentCultureIgnoreCase)) ToggleFullscreen();
        }


        private static void ToggleFullscreen()
        {
            Settings.Fullscreen = !Settings.Fullscreen;
            var type = "Bounds";
            if (Settings.Fullscreen) type = "WindowState";
            var item = Shutter.Tiles.FirstOrDefault(s => string.Equals(s.Command, "Fullscreen", StringComparison.CurrentCultureIgnoreCase));
            if (item != null)
            {
                item.DisplayOptions = type;
            }

            VideoCapture.Home.SetFullscreen();
        }
    }
}
