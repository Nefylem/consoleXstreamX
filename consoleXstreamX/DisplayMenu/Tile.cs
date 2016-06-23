using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Configuration;
using consoleXstreamX.Drawing;

namespace consoleXstreamX.DisplayMenu
{
    internal static class Tile
    {
        public static void Create(Rectangle rect, MenuSettings.MenuItems tile, bool inactive = false)
        {
            /*
            //Active
            if (_class.Data.Buttons.Any(t => String.Equals(t.Command, strCommand, StringComparison.CurrentCultureIgnoreCase)))
            return;

            _class.Data.Buttons.Add(new ButtonItem(_class));
            var intIndex = _class.Data.Buttons.Count - 1;

            _class.Data.Buttons[intIndex].Command = strCommand;
            _class.Data.Buttons[intIndex].Rect = rect;
            */
            /*
            foreach (var t in _class.Data.InactiveButtons)
            {
                if (String.Equals(t.Command, strCommand, StringComparison.CurrentCultureIgnoreCase))
                    return;
            }

            _class.Data.InactiveButtons.Add(new ButtonItem(_class));
            var intIndex = _class.Data.InactiveButtons.Count - 1;

            _class.Data.InactiveButtons[intIndex].Command = strCommand;
            _class.Data.InactiveButtons[intIndex].Rect = rect;
            */
            if (string.Equals(MenuSettings.Selected, tile.Command, StringComparison.CurrentCultureIgnoreCase))
                Draw.Image(rect.X - 6, rect.Y - 7, 108, 115, Properties.Resources.tile_high);
            else
                Draw.Image(rect.X, rect.Y, Properties.Resources.tile_low);

            Draw.Text(rect, tile.Display);
            /*
            if (_class.User.Selected == t1.Command)
                _class.DrawGui.DrawImage(intX - 6, intY - 7, 108, 115, Properties.Resources.imgSubGlow);
            else
                _class.DrawGui.DrawImage(intX, intY, Properties.Resources.imgTileLow);

            _class.DrawGui.CenterText(displayRect, t1.Display);
            */
        }
    }
}
