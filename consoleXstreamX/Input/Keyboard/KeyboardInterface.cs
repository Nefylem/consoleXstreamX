using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Define;
using consoleXstreamX.Remapping;

namespace consoleXstreamX.Input.Keyboard
{
    internal static class KeyboardInterface
    {
        private static int _xboxButtonCount = 0;

        public static Gamepad.GamepadOutput Read()
        {
            
            if (_xboxButtonCount == 0) _xboxButtonCount = Enum.GetNames(typeof(Xbox)).Length;
            var output = new byte[_xboxButtonCount];

            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Up) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Up)) output[GamepadMap.Up] = Convert.ToByte(100);
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Down) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Down)) output[GamepadMap.Down] = Convert.ToByte(100);
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Left) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Left)) output[GamepadMap.Left] = Convert.ToByte(100);
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Right) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Right)) output[GamepadMap.Right] = Convert.ToByte(100);

            if (KeyHook.GetKey(KeyboardMap.KeyDefine.A) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.A)) output[GamepadMap.A] = Convert.ToByte(100);
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.B) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.B)) output[GamepadMap.B] = Convert.ToByte(100);
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.X) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.X)) output[GamepadMap.X] = Convert.ToByte(100);
            if (KeyHook.GetKey(KeyboardMap.KeyDefine.Y) || KeyHook.GetKey(KeyboardMap.AltKeyDefine.Y)) output[GamepadMap.Y] = Convert.ToByte(100);


            return new Gamepad.GamepadOutput()
            {
                PlayerIndex = PlayerIndex.One,
                Output = output,
                Index = 1
            };
        }
    }
}
