using System.Drawing;
using consoleXstreamX.Configuration;
using consoleXstreamX.Drawing;

namespace consoleXstreamX.DisplayMenu
{
    internal static class DrawMain
    {
        public static void Execute()
        {
            var x = 10 + MenuSettings.OffsetX;
            var y = 10 + MenuSettings.OffsetY;

            MenuSettings.CellHeight = Properties.Resources.tile_low.Height;
            MenuSettings.CellWidth = Properties.Resources.tile_low.Width;

            Draw.FontSize = 18f;
            Draw.Outline = true;
            Draw.SetVertical = Draw.VerticalAlignment.Bottom;
            Draw.SetHorizontal = Draw.HorizontalAlignment.Middle;

            foreach (var row in MenuSettings.Tiles)
            {
                foreach (var tile in row)
                {
                    var rect = new Rectangle(x, y, MenuSettings.CellWidth, MenuSettings.CellHeight);

                    if (MenuSettings.MainMenu)
                        Tile.Create(rect, tile);
                    else
                        Tile.Create(rect, tile, true);       //Add to the inactive list incase its needed - mainly by mouse move

                    x += MenuSettings.CellWidth + 5;
                }
                y += MenuSettings.CellHeight + 5;
                x = 10;
            }
            Draw.Outline = true;
            Draw.FontSize = 12f;

            /*
            //_class.Data.Row.Clear();

            foreach (var t in _class.Data.Items)
            {
                _class.Data.Row.Add(y);

                foreach (var t1 in t)
                {
                    var displayRect = new Rectangle(x, y, MenuSettings.CellWidth, MenuSettings.CellHeight);

                    if (_class.Var.IsMainMenu)
                        _class.Button.Create(displayRect, t1.Command);
                    else
                        _class.Button.Create(displayRect, t1.Command, false);       //Add to the inactive list incase its needed - mainly by mouse move

                    if (_class.User.Selected == t1.Command)
                        _class.DrawGui.DrawImage(intX - 6, intY - 7, 108, 115, Properties.Resources.imgSubGlow);
                    else
                        _class.DrawGui.DrawImage(intX, intY, Properties.Resources.imgTileLow);

                    _class.DrawGui.CenterText(displayRect, t1.Display);

                    x += MenuSettings.CellWidth + 5;
                }
                y += MenuSettings.CellHeight + 5;
                x = 10;
            }
            Draw.Outline = true;
            Draw.FontSize = 12f;
            */
        }
    }
}
