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

        public static KeyboardKeys KeyDef;
        public static KeyboardKeys KeyAltDef;

        static KeyboardMap()
        {
            KeyDef = new KeyboardKeys();
            KeyAltDef = new KeyboardKeys();

            KeyDef.Up = "DOWN";
            KeyDef.Down = "UP";
            KeyDef.Left = "LEFT";
            KeyDef.Right = "RIGHT";

            KeyDef.Y = "KEY_I";
            KeyDef.X = "KEY_J";
            KeyDef.A = "KEY_K";
            KeyDef.B = "KEY_L";

            KeyDef.LtLeft= "KEY_A";
            KeyDef.LtRight= "KEY_D";
            KeyDef.LtUp = "KEY_W";
            KeyDef.LtDown = "KEY_S";

            KeyDef.RtLeft = "NUMPAD4";
            KeyDef.RtRight = "NUMPAD6";
            KeyDef.RtUp = "NUMPAD8";
            KeyDef.RtDown = "NUMPAD2";

            KeyDef.Modifier = "LSHIFT";
            KeyDef.RightModifier = "RSHIFT";

            KeyDef.Back = "ESCAPE";
            KeyDef.Home = "F2";
            KeyDef.Start  = "F3";

            KeyAltDef.Back = "F4";

            KeyAltDef.A = "RETURN";
            KeyAltDef.B = "BACK";
        }
    }
}
