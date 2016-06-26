using System.Collections.Generic;
using consoleXstreamX.Configuration;

namespace consoleXstreamX.DisplayMenu.SubMenu
{
    internal static class Shutter
    {
        public static List<MenuSettings.MenuItems> Tiles;
        public static List<string> CheckedItems; 
        public static int Scroll;
        public static string Error;
        public static string Explanation;
        public static string Selected;

        public static int TargetColumn;
        public static int Start;
        public static int End;

        public static int Width;
        public static int Height;
        public static int SlideSpeed;
        public static bool Active;

        public static bool Hide
        {
            get { return _hide; }
            set
            {
                _hide = value;
                _open = !value;
            }
        }
        public static bool Open
        {
            get { return _open; }
            set
            {
                if (value) Active = true;
                _open = value;
                _hide = !value;
            }
        }

        private static bool _open;
        private static bool _hide;

        static Shutter()
        {
            Width = Properties.Resources.shutter.Width;
            Tiles = new List<MenuSettings.MenuItems>();
            CheckedItems = new List<string>();
        }

        public static void AddItem(string title, string command, string options = "", string active = "", bool folder = false)
        {
            Tiles.Add(new MenuSettings.MenuItems()
            {
                Display = title,
                Command = command,
                DisplayOptions = options,
                ActiveFolder = active,
                Folder = folder
            });
        }

        public static void SetActiveRow(int column)
        {
            var current = MenuCommand.FindCurrentObject();
            TargetColumn = current.Column + column;

            Start = Display.RowPosition[TargetColumn];
            End = Start + MenuSettings.CellHeight;
            Height = 0;

            if (Fps.Count > 0)
                SlideSpeed = Fps.Count/3;
            else
                SlideSpeed = 10;

            Open = true;
        }

        public static void Check()
        {
            if (!Active) return;

            if (Open)
            {
                if (Height >= End - Start) return;
                Height += SlideSpeed;

                if (Height > End - Start)
                    Height = End - Start;
                return;
            }

            if (!Hide) return;
            if (Height > 0) Height -= SlideSpeed;
            if (Height <= 0)
            {
                Active = false;
            }
        }

    }
}
