using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Input.Keyboard;
using consoleXstreamX.Remapping;

namespace consoleXstreamX.DisplayMenu
{
    internal static class KeyboardInput
    {
        public static void Check()
        {
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Up) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Up)) MenuCommand.Execute("up");
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Down) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Down)) MenuCommand.Execute("down");
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Left) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Left)) MenuCommand.Execute("left");
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Right) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Right)) MenuCommand.Execute("right");
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.B) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.B)) MenuCommand.Execute("back");
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Back) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Back)) MenuCommand.Execute("back");
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.A) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.A)) MenuCommand.Execute("ok");
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Start) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Start)) MenuCommand.Execute("ok");
        }
    }
}
