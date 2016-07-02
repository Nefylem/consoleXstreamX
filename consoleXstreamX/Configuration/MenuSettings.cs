using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleXstreamX.Configuration
{
    internal static class MenuSettings
    {
        public static int OffsetX;
        public static int OffsetY;

        public static int CellWidth;
        public static int CellHeight;

        public static int MinimumFps;
        public static int FpsModifier;
        public static bool ShowFps;

        public static string CurrentMenu;
        public static string Selected;

        public static List<string> History;
        public static List<List<MenuItems>> Tiles;

        public class MenuItems
        {
            public string Display;
            public string Command;
            public string DisplayOptions;
            public string ActiveFolder;
            public bool Folder;
        }

        static MenuSettings()
        {
            Tiles = new List<List<MenuItems>>();

            MinimumFps = 20;
            FpsModifier = 6;
            ShowFps = true;

            OffsetX = 0;
            OffsetY = 0;
        }

        /*
        public bool Setup;
        public bool SetupGamepad;
        public bool ShowSubSelection;
        */
    }
}
