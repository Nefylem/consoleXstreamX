using System;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;
using consoleXstreamX.DisplayMenu.SubMenu;

namespace consoleXstreamX.DisplayMenu
{
    internal static class MenuCommand
    {
        private static int _upWait;
        private static int _downWait;
        private static int _leftWait;
        private static int _rightWait;
        private static int _okWait;
        private static int _backWait;

        public static int OkWait
        {
            get { return _okWait; }
            set { _okWait = value; }
        }

        public static int BackWait
        {
            
            get { return _backWait; }
            set { _backWait = value; }
        }

        public static void Execute(string command)
        {
            if (Shutter.Active)
            {
                ShutterCommand.Execute(command);
                return;
            }
            if (string.Equals(command, "up", StringComparison.CurrentCultureIgnoreCase)) MoveUp();
            if (string.Equals(command, "down", StringComparison.CurrentCultureIgnoreCase)) MoveDown();
            if (string.Equals(command, "left", StringComparison.CurrentCultureIgnoreCase)) MoveLeft();
            if (string.Equals(command, "right", StringComparison.CurrentCultureIgnoreCase)) MoveRight();
            if (string.Equals(command, "ok", StringComparison.CurrentCultureIgnoreCase)) SetOk();
        }

        private static void SetOk()
        {
            if (_okWait > 0)
            {
                OkWait = 5;
                return;
            }
            OkWait = 5;
            MenuActions.Run(MenuSettings.Selected);
        }

        private static void MoveUp()
        {
            if (_upWait > 0) return;
            var current = FindCurrentObject();
            if (current == null) return;

            if (current.Column == 0) return;
            if (MenuSettings.Tiles[current.Column - 1].Count <= current.Row) return;
            MenuSettings.Selected = MenuSettings.Tiles[current.Column - 1][current.Row].Command;
            _upWait = SetMoveWait();
        }

        private static void MoveDown()
        {
            if (_downWait > 0) return;
            var current = FindCurrentObject();
            if (current == null) return;

            if (current.Column >= MenuSettings.Tiles.Count - 1) return;
            if (MenuSettings.Tiles[current.Column + 1].Count <= current.Row) return;
            MenuSettings.Selected = MenuSettings.Tiles[current.Column + 1][current.Row].Command;
            _downWait = SetMoveWait();
        }

        private static void MoveLeft()
        {
            if (_leftWait > 0) return;
            var current = FindCurrentObject();
            if (current == null) return;

            if (current.Row == 0) return;
            MenuSettings.Selected = MenuSettings.Tiles[current.Column][current.Row - 1].Command;
            _leftWait = SetMoveWait();
        }

        private static void MoveRight()
        {
            if (_rightWait > 0) return;
            var current = FindCurrentObject();
            if (current == null) return;

            if (current.Row >= MenuSettings.Tiles[current.Column].Count - 1) return;
            MenuSettings.Selected = MenuSettings.Tiles[current.Column][current.Row + 1].Command;
            _rightWait = SetMoveWait();
        }

        public static int SetMoveWait()
        {
            if (Fps.Count > MenuSettings.MinimumFps) return Fps.Count / MenuSettings.FpsModifier;
            return 3;
        }

        public static void CheckDelays()
        {
            if (_upWait > 0) _upWait--;
            if (_downWait > 0) _downWait--;
            if (_leftWait > 0) _leftWait--;
            if (_rightWait > 0) _rightWait--;
            if (_okWait > 0) _okWait--;
            if (_backWait > 0) _backWait--;
            ShutterCommand.CheckDelays();
        }

        public static CurrentItem FindCurrentObject()
        {
            for (var col = 0; col < MenuSettings.Tiles.Count; col++)
            {
                var row = MenuSettings.Tiles[col];
                for (var tile = 0; tile < row.Count; tile++)
                {
                    if (string.Equals(row[tile].Command, MenuSettings.Selected, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return new CurrentItem()
                        {
                            Column = col,
                            Row = tile
                        };
                    }
                }
            }
            return null;
        }

        internal class CurrentItem
        {
            public int Column;
            public int Row;
        }
    }
}
