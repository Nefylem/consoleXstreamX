using System;
using consoleXstreamX.Configuration;
using consoleXstreamX.Define;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Remapping;

namespace consoleXstreamX.Input
{
    internal static class Gamepad
    {
        public static int PressHome;
        private static int _xboxButtonCount = 0;

        public class GamepadOutput
        {
            public int Index;
            public byte[] Output;
            public PlayerIndex PlayerIndex;
        }

        public static GamepadOutput Check(int index)
        {
            if (_xboxButtonCount == 0) _xboxButtonCount = Enum.GetNames(typeof(Xbox)).Length;
            var output = new byte[_xboxButtonCount];

            var player = FindPlayerIndex(index);
            var controls = GamePad.GetState(player);

            if (controls.DPad.Left) { output[GamepadMap.Left] = Convert.ToByte(100); }
            if (controls.DPad.Right) { output[GamepadMap.Right] = Convert.ToByte(100); }
            if (controls.DPad.Up) { output[GamepadMap.Up] = Convert.ToByte(100); }
            if (controls.DPad.Down) { output[GamepadMap.Down] = Convert.ToByte(100); }

            if (controls.Buttons.A) output[GamepadMap.A] = Convert.ToByte(100);
            if (controls.Buttons.B) output[GamepadMap.B] = Convert.ToByte(100);
            if (controls.Buttons.X) output[GamepadMap.X] = Convert.ToByte(100);
            if (controls.Buttons.Y) output[GamepadMap.Y] = Convert.ToByte(100);

            if (controls.Buttons.LeftShoulder) { output[GamepadMap.LeftShoulder] = Convert.ToByte(100); }
            if (controls.Buttons.RightShoulder) { output[GamepadMap.RightShoulder] = Convert.ToByte(100); }
            if (controls.Buttons.LeftStick) { output[GamepadMap.LeftStick] = Convert.ToByte(100); }
            if (controls.Buttons.RightStick) { output[GamepadMap.RightStick] = Convert.ToByte(100); }

            if (controls.Triggers.Left > 0) { output[GamepadMap.LeftTrigger] = Convert.ToByte(controls.Triggers.Left * 100); }
            if (controls.Triggers.Right > 0) { output[GamepadMap.RightTrigger] = Convert.ToByte(controls.Triggers.Right * 100); }

            double leftX = controls.ThumbSticks.Left.X * 100;
            double leftY = controls.ThumbSticks.Left.Y * 100;
            double rightX = controls.ThumbSticks.Right.X * 100;
            double rightY = controls.ThumbSticks.Right.Y * 100;

            if (Settings.NormalizeControls)
            {
                NormalGamepad(ref leftX, ref leftY);
                NormalGamepad(ref rightX, ref rightY);
            }
            else
            {
                leftY = -leftY;
                rightY = -rightY;
            }

            if (Math.Abs(leftX) > 0) { output[GamepadMap.LeftX] = (byte)Convert.ToSByte((int)(leftX)); }
            if (Math.Abs(leftY) > 0) { output[GamepadMap.LeftY] = (byte)Convert.ToSByte((int)(leftY)); }
            if (Math.Abs(rightX) > 0) { output[GamepadMap.RightX] = (byte)Convert.ToSByte((int)(rightX)); }
            if (Math.Abs(rightY) > 0) { output[GamepadMap.RightY] = (byte)Convert.ToSByte((int)(rightY)); }

            if (PressHome > 0)      //Send from menu
            {
                output[GamepadMap.Home] = Convert.ToByte(100);
                PressHome--;
            }

            if (controls.Buttons.Guide) { output[GamepadMap.Home] = Convert.ToByte(100); }
            if (controls.Buttons.Start) { output[GamepadMap.Start] = Convert.ToByte(100); }
            if (controls.Buttons.Back)
            {
                if (!Settings.BlockMenuCommand)
                {
                    MenuController.Delay++;
                    if (!MenuController.Visible)
                        if (MenuController.Delay >= MenuController.DelayLimit) MenuController.Open();
                }

                if (Settings.Ps4ControllerMode)
                    output[GamepadMap.Touch] = Convert.ToByte(100);
                else
                    output[GamepadMap.Back] = Convert.ToByte(100);
            }
            else
            {
                MenuController.Delay = 0;
            }

            if (Settings.UseShortcutKeys) Shortcuts.Check(ref output);

            return new GamepadOutput()
            {
                Output = output,
                Index = index,
                PlayerIndex = player                    //Store this for returning rumble
            };
        }

        //Output is -100 to 100 in both x and y. Reading from a gamepad is circular, output on angles is more like 75, 75 instead. 
        //This turns a circle into a square basically.
        private static void NormalGamepad(ref double dblLx, ref double dblLy)
        {
            var dblNewX = dblLx;
            var dblNewY = dblLy;

            var dblLength = Math.Sqrt(Math.Pow(dblLx, 2) + Math.Pow(dblLy, 2));
            if (dblLength > 99.9)
            {
                var dblTheta = Math.Atan2(dblLy, dblLx);
                var dblAngle = (90 - ((dblTheta * 180) / Math.PI)) % 360;

                if ((dblAngle < 0) && (dblAngle >= -45)) { dblNewX = (int)(100 / Math.Tan(dblTheta)); dblNewY = -100; }
                if ((dblAngle >= 0) && (dblAngle <= 45)) { dblNewX = (int)(100 / Math.Tan(dblTheta)); dblNewY = -100; }
                if ((dblAngle > 45) && (dblAngle <= 135)) { dblNewY = -(int)(Math.Tan(dblTheta) * 100); dblNewX = 100; }
                if ((dblAngle > 135) && (dblAngle <= 225)) { dblNewX = -(int)(100 / Math.Tan(dblTheta)); dblNewY = 100; }
                if (dblAngle > 225) { dblNewY = (int)(Math.Tan(dblTheta) * 100); dblNewX = -100; }
                if (dblAngle < -45) { dblNewY = (int)(Math.Tan(dblTheta) * 100); dblNewX = -100; }
            }
            else
            {
                dblNewY = -dblNewY;
            }

            //Return values
            dblLx = dblNewX;
            dblLy = dblNewY;
        }

        private static PlayerIndex FindPlayerIndex(int index)
        {
            switch (index)
            {
                case 2: { return PlayerIndex.Two; }
                case 3: { return PlayerIndex.Three; }
                case 4: { return PlayerIndex.Four; }
            }
            return PlayerIndex.One;
        }

    }
}
