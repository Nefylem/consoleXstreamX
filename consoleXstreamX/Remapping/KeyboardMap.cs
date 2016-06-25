using System.ComponentModel;

namespace consoleXstreamX.Remapping
{
    internal static class KeyboardMap
    {
        public class KeyboardKeys
        {
            [Description("D-Pad Up")] public string Up;
            [Description("D-Pad Down")] public string Down;
            [Description("D-Pad Left")] public string Left;
            [Description("D-Pad Right")] public string Right;
            [Description("A")] public string A;
            [Description("B")] public string B;
            [Description("X")] public string X;
            [Description("Y")] public string Y;
            [Description("RightThumb Left")] public string RtLeft;
            [Description("RightThumb Right")] public string RtRight;
            [Description("RightThumb Up")] public string RtUp;
            [Description("RightThumb Down")] public string RtDown;
            [Description("LeftThumb Left")] public string LtLeft;
            [Description("LeftThumb Right")] public string LtRight;
            [Description("LeftThumb Up")] public string LtUp;
            [Description("LeftThumb Down")] public string LtDown;
            [Description("LeftThumb Modifier")] public string Modifier;
            [Description("RightThumb Modifier")] public string RightModifier;
            [Description("Back")] public string Back;
            [Description("Start")] public string Start;
            [Description("Home")] public string Home;
        }

        public static KeyboardKeys KeyDefine;
        public static KeyboardKeys AltKeyDefine;

        static KeyboardMap()
        {
            KeyDefine = new KeyboardKeys();
            AltKeyDefine = new KeyboardKeys();

            KeyDefine.Up = "UP";
            KeyDefine.Down = "DOWN";
            KeyDefine.Left = "LEFT";
            KeyDefine.Right = "RIGHT";

            KeyDefine.Y = "KEY_I";
            KeyDefine.X = "KEY_J";
            KeyDefine.A = "KEY_K";
            KeyDefine.B = "KEY_L";

            KeyDefine.LtLeft = "KEY_A";
            KeyDefine.LtRight = "KEY_D";
            KeyDefine.LtUp = "KEY_W";
            KeyDefine.LtDown = "KEY_S";

            KeyDefine.RtLeft = "NUMPAD4";
            KeyDefine.RtRight = "NUMPAD6";
            KeyDefine.RtUp = "NUMPAD8";
            KeyDefine.RtDown = "NUMPAD2";

            KeyDefine.Modifier = "LSHIFT";
            KeyDefine.RightModifier = "RSHIFT";

            KeyDefine.Back = "ESCAPE";
            KeyDefine.Home = "F2";
            KeyDefine.Start  = "F3";

            AltKeyDefine.Back = "F4";
            AltKeyDefine.A = "RETURN";
            AltKeyDefine.B = "BACK";
        }
    }
}
