using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleXstreamX.Remapping
{
    public static class GamepadMap
    {
        [Description("Home")] public static int Home;
        [Description("Back")] public static int Back;
        [Description("Start")] public static int Start;
        [Description("Right Shoulder")] public static int RightShoulder;
        [Description("Right Trigger")] public static int RightTrigger;          //0 - 100
        [Description("Right Stick")] public static int RightStick;
        [Description("Left Shoulder")] public static int LeftShoulder;
        [Description("Left Trigger")] public static int LeftTrigger;            //0 - 100
        [Description("Left Stick")] public static int LeftStick;
        [Description("RightThumb X")] public static int RightX;
        [Description("RightThumb Y")] public static int RightY;
        [Description("LeftThumb X")] public static int LeftX;
        [Description("LeftThumb Y")] public static int LeftY;
        [Description("D-Pad Up")] public static int Up;
        [Description("D-Pad Down")] public static int Down;
        [Description("D-Pad Left")] public static int Left;
        [Description("D-Pad Right")] public static int Right;
        [Description("A")] public static int A;
        [Description("B")] public static int B;
        [Description("X")] public static int X;
        [Description("Y")] public static int Y;
        [Description("Accelerometer X")] public static int AccX;                //rotate X. 90 = -25, 180 = 0, 270 = +25, 360 = 0
        [Description("Accelerometer Y")] public static int AccY;                //shake vertically. +25 (top) to -25 (bottom)
        [Description("Accelerometer Z")] public static int AccZ;                //tilt
        [Description("Gyro X")] public static int GyroX;        
        [Description("Gyro Y")] public static int GyroY;        
        [Description("Gyro Z")] public static int GyroZ;        
        [Description("Touchpad")] public static int Touch;
        [Description("Touchpad X")] public static int TouchX;
        [Description("Touchpad Y")] public static int TouchY;

        static GamepadMap()
        {
            Home = 0;
            Back = 1;
            Start = 2;
            RightShoulder = 3;
            RightTrigger = 4;
            RightStick = 5;
            LeftShoulder = 6;
            LeftTrigger = 7;
            LeftStick = 8;
            RightX = 9;
            RightY = 10;
            LeftX = 11;
            LeftY = 12;
            Up = 13;
            Down = 14;
            Left = 15;
            Right = 16;
            Y = 17;
            B = 18;
            A = 19;
            X = 20;
            Touch = 27;
            TouchX = 28;
            TouchY = 29;
        }
    }
}
