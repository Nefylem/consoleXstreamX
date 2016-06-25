using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Define;

namespace consoleXstreamX.DisplayMenu
{
    internal static class GamepadInput
    {
        public static void Check()
        {
            var controls = GamePad.GetState(PlayerIndex.One);
            if (controls.DPad.Up) MenuCommand.Execute("up");
            if (controls.DPad.Down) MenuCommand.Execute("down");
            if (controls.DPad.Left) MenuCommand.Execute("left");
            if (controls.DPad.Right) MenuCommand.Execute("right");
            if (controls.Buttons.B || controls.Buttons.Back) MenuCommand.Execute("back");
            if (controls.Buttons.A || controls.Buttons.Start) MenuCommand.Execute("ok");
        }
    }
}
