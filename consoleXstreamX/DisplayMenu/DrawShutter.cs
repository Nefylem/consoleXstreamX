using System;
using System.Drawing;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.SubMenu;
using consoleXstreamX.Drawing;

namespace consoleXstreamX.DisplayMenu
{
    internal static class DrawShutter
    {
        public static void Execute()
        {
            if (!Shutter.Active) return;
            if (Shutter.Height == 0) return;

            var image = new Bitmap(Shutter.Width, Shutter.Height);
            Draw.Image(0, 0, Properties.Resources.shutter, image);
            if (string.IsNullOrEmpty(Shutter.Error))
                DrawTiles(image);
            else
                DrawError(image);

            Draw.Image(new Rectangle(8, Shutter.Start, 581, Shutter.Height), image);
        }

        private static void DrawTiles(Bitmap image)
        {
            var x = 0;

            if (Shutter.Tiles.Count < 4)
            {
                var tileWidth = Shutter.Tiles.Count * (MenuSettings.CellWidth + 5);
                x = ((Shutter.Width - 20) / 2) - (tileWidth / 2);
            }

            for (var count = Shutter.Scroll; count < Shutter.Tiles.Count; count++)
            {
                var tile = Shutter.Tiles[count];

                var displayTile = new Rectangle(x, 2, MenuSettings.CellWidth, MenuSettings.CellHeight - 4);
                var displayText = new Rectangle(x, 2, MenuSettings.CellWidth, MenuSettings.CellHeight - 24);
                var buttonRect = new Rectangle(x + 8, Shutter.Start, MenuSettings.CellWidth, MenuSettings.CellHeight - 24);

                if (string.Equals(Shutter.Selected, tile.Command, StringComparison.CurrentCultureIgnoreCase))
                    Draw.Image(displayTile, Properties.Resources.tile_high, image);

                if (tile.Folder)
                {
                    /*
                    //draw folder option (three dots)
                    _class.DrawGui.drawImage(bmpShutter, intX + _class.Var.CellWidth - 50, 90, 60, 60, Properties.Resources.ThreeDots);
                    var displayRectTextOption = new Rectangle(intX, 2, _class.Var.CellWidth, _class.Var.CellHeight - 60);
                    var displayRectTextDisplay = new Rectangle(intX, 2, _class.Var.CellWidth, _class.Var.CellHeight - 34);
                    _class.DrawGui.CenterText(bmpShutter, displayRectTextOption, _class.Data.SubItems[intCount].DisplayOption);
                    _class.DrawGui.CenterText(bmpShutter, displayRectTextDisplay, _class.Data.SubItems[intCount].Display);
                    */
                }
                else
                {
                    if (string.IsNullOrEmpty(tile.DisplayOptions))
                    {
                        Draw.FontSize = 12f;
                        Draw.Text(displayText, tile.Display, image);
                    }
                    else
                    {
                        Draw.FontSize = 12f;
                        Draw.Text(displayText, tile.Display + "\n" + tile.DisplayOptions, image);

                        /*
                        var displayRectTextOption = new Rectangle(intX, 2, _class.Var.CellWidth, _class.Var.CellHeight - 60);
                        //var displayRectTextDisplay = new Rectangle(intX, 2, _class.Var.CellWidth, _class.Var.CellHeight - 34);
                        _class.DrawGui.CenterText(bmpShutter, displayRectText, _class.Data.SubItems[intCount].Display);
                        _class.DrawGui.CenterText(bmpShutter, displayRectTextOption, _class.Data.SubItems[intCount].DisplayOption);
                        */
                    }
                }

                if (Shutter.CheckedItems.IndexOf(tile.Command) > -1) Draw.Image(displayTile.X + displayTile.Width - 40, 17, 25, 25, Properties.Resources.tick, image);

                /*
                //Check for changing information
                if (_class.Data.SubItems[intCount].ActiveWatcher.Length > 0)
                    _class.Data.SubItems[intCount].DisplayOption = _class.SubAction.FindSubOption(_class.Data.SubItems[intCount].Display);
                    */
                x += MenuSettings.CellWidth + 5;
            }
        }

        private static void DrawError(Bitmap image)
        {
            Draw.SetVertical = Draw.VerticalAlignment.Top;
            Draw.SetHorizontal = Draw.HorizontalAlignment.Middle;
            Draw.FontSize = 24f;
            Draw.Outline = true;
            Draw.Text(new Rectangle(-40, 20, Shutter.Width - 20, Shutter.Height - 20), Shutter.Error, image);

            if (string.IsNullOrEmpty(Shutter.Explanation)) return;
            Draw.FontSize = 14f;
            Draw.Text(new Rectangle(-50, 80, Shutter.Width - 20, Shutter.Height - 60), Shutter.Explanation, image);
        }
    }
}
