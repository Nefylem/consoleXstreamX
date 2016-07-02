using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace consoleXstreamX.Define
{
    public enum Xbox
    {
        [Description("Home")]
        Home = 0,
        [Description("Back")]
        Back = 1,
        [Description("Start")]
        Start = 2,
        [Description("Right Shoulder")]
        RightShoulder = 3,
        [Description("Right Trigger")]
        RightTrigger = 4,
        [Description("Right Stick")]
        RightStick = 5,
        [Description("Left Shoulder")]
        LeftShoulder = 6,
        [Description("Left Trigger")]
        LeftTrigger = 7,
        [Description("Left Stick")]
        LeftStick = 8,
        [Description("RightThumb X")]
        RightX = 9,
        [Description("RightThumb Y")]
        RightY = 10,
        [Description("LeftThumb X")]
        LeftX = 11,
        [Description("LeftThumb Y")]
        LeftY = 12,
        [Description("D-Pad Up")]
        Up = 13,
        [Description("D-Pad Down")]
        Down = 14,
        [Description("D-Pad Left")]
        Left = 15,
        [Description("D-Pad Right")]
        Right = 16,
        [Description("Y")]
        Y = 17,
        [Description("B")]
        B = 18,
        [Description("A")]
        A = 19,
        [Description("X")]
        X = 20,
        AccX = 21,      //rotate X. 90 = -25, 180 = 0, 270 = +25, 360 = 0 (ng)
        AccY = 22,      //shake vertically. +25 (top) to -25 (bottom) (ng)
        AccZ = 23,      //tilt up
        GyroX = 24,     //no reading
        GyroY = 25,     //no reading
        GyroZ = 26,     //no reading
        [Description("Touchpad")]
        Touch = 27,             //touchpad, 100 = on    (works)
        TouchX = 28,            //-100 to 100   (left to right)
        TouchY = 29             //-100 to 100   (top to bottom)
    }                               //Control codes for incoming gamepad 

    public enum BatteryTypes : byte
    {
        //
        // Flags for battery status level
        //
        BatteryTypeDisconnected = 0x00,    // This device is not connected
        BatteryTypeWired = 0x01,    // Wired device, no battery
        BatteryTypeAlkaline = 0x02,    // Alkaline battery source
        BatteryTypeNimh = 0x03,    // Nickel Metal Hydride battery source
        BatteryTypeUnknown = 0xFF,    // Cannot determine the battery type
    };

    public enum BatteryLevel : byte
    {
        BatteryLevelEmpty = 0x00,
        BatteryLevelLow = 0x01,
        BatteryLevelMedium = 0x02,
        BatteryLevelFull = 0x03
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct XInputBatteryInformation
    {
        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(0)]
        public byte BatteryType;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(1)]
        public byte BatteryLevel;

        public override string ToString()
        {
            return $"{(BatteryTypes) BatteryType} {(BatteryLevel) BatteryLevel}";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputVibration
    {
        [MarshalAs(UnmanagedType.I2)]
        public ushort LeftMotorSpeed;

        [MarshalAs(UnmanagedType.I2)]
        public ushort RightMotorSpeed;
    }

    class Imports
    {
        [DllImport("xinput1_3.dll")]
        public static extern int XInputSetState
        (
            int dwUserIndex,  // [in] Index of the gamer associated with the device
            ref XInputVibration pVibration    // [in, out] The vibration information to send to the controller
        );

        [DllImport("xinput1_3.dll")]
        public static extern int XInputGetBatteryInformation
        (
              int dwUserIndex,        // Index of the gamer associated with the device
              byte devType,            // Which device on this user index
              ref XInputBatteryInformation pBatteryInformation // Contains the level and types of batteries
        );

        [DllImport("xinput1_3.dll", EntryPoint = "#100")]
        public static extern uint XInputGetState(uint playerIndex, IntPtr state);
        [DllImport("xinput1_3.dll", EntryPoint = "#103")]
        public static extern uint PowerOff(uint playerIndex);

        public enum Constants
        {
            Success = 0x000,
            NotConnected = 0x48F,
            LeftStickDeadZone = 7849,
            RightStickDeadZone = 8689
        }
    }

    public enum ButtonState
    {
        Pressed,
        Released
    }
    public struct GamePadButtons
    {
        ButtonState start, back, leftStick, rightStick, leftShoulder, rightShoulder, guide, a, b, x, y;

        internal GamePadButtons(ButtonState start, ButtonState back, ButtonState leftStick, ButtonState rightStick,
                                ButtonState leftShoulder, ButtonState rightShoulder, ButtonState guide, ButtonState a, ButtonState b,
                                ButtonState x, ButtonState y)
        {
            this.start = start;
            this.back = back;
            this.guide = guide;
            this.leftStick = leftStick;
            this.rightStick = rightStick;
            this.leftShoulder = leftShoulder;
            this.rightShoulder = rightShoulder;
            this.a = a;
            this.b = b;
            this.x = x;
            this.y = y;
        }

        public bool Start
        {
            get { return start == ButtonState.Pressed; }
        }

        public bool Back
        {
            get { return back == ButtonState.Pressed; }
        }

        public bool Guide
        {
            get { return guide == ButtonState.Pressed; }
        }

        public bool LeftStick
        {
            get { return leftStick == ButtonState.Pressed; }
        }

        public bool RightStick
        {
            get { return rightStick == ButtonState.Pressed; }
        }

        public bool LeftShoulder
        {
            get { return leftShoulder == ButtonState.Pressed; }
        }

        public bool RightShoulder
        {
            get { return rightShoulder == ButtonState.Pressed; }
        }

        public bool A
        {
            get { return a == ButtonState.Pressed; }
        }

        public bool B
        {
            get { return b == ButtonState.Pressed; }
        }

        public bool X
        {
            get { return x == ButtonState.Pressed; }
        }

        public bool Y
        {
            get { return y == ButtonState.Pressed; }
        }
    }

    public struct GamePadDPad
    {
        ButtonState up, down, left, right;

        internal GamePadDPad(ButtonState up, ButtonState down, ButtonState left, ButtonState right)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public bool Up
        {
            get { return up == ButtonState.Pressed; }
        }

        public bool Down
        {
            get { return down == ButtonState.Pressed; }
        }

        public bool Left
        {
            get { return left == ButtonState.Pressed; }
        }

        public bool Right
        {
            get { return right == ButtonState.Pressed; }
        }
    }

    public struct GamePadThumbSticks
    {
        public struct StickValue
        {
            float x, y;

            internal StickValue(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public float X
            {
                get { return x; }
            }

            public float Y
            {
                get { return y; }
            }
        }

        StickValue left, right;

        internal GamePadThumbSticks(StickValue left, StickValue right)
        {
            this.left = left;
            this.right = right;
        }

        public StickValue Left
        {
            get { return left; }
        }

        public StickValue Right
        {
            get { return right; }
        }
    }

    public struct GamePadTriggers
    {
        float left;
        float right;

        internal GamePadTriggers(float left, float right)
        {
            this.left = left;
            this.right = right;
        }

        public float Left
        {
            get { return left; }
        }

        public float Right
        {
            get { return right; }
        }
    }

    public struct GamePadState
    {
        public struct RawState
        {
            public uint DwPacketNumber;
            public GamePad Gamepad;

            public struct GamePad
            {
                public ushort DwButtons;
                public byte BLeftTrigger;
                public byte BRightTrigger;
                public short SThumbLx;
                public short SThumbLy;
                public short SThumbRx;
                public short SThumbRy;
            }
        }

        bool isConnected;
        uint packetNumber;
        GamePadButtons buttons;
        GamePadDPad dPad;
        GamePadThumbSticks thumbSticks;
        GamePadTriggers triggers;
        RawState currentRawState;

        enum ButtonsConstants
        {
            DPadUp = 0x0001,
            DPadDown = 0x0002,
            DPadLeft = 0x0004,
            DPadRight = 0x0008,
            Start = 0x10,
            Back = 0x20,
            Guide = 0x0400,
            LeftThumb = 0x00000040,
            RightThumb = 0x00000080,
            LeftShoulder = 0x0100,
            RightShoulder = 0x0200,
            A = 0x1000,
            B = 0x2000,
            X = 0x4000,
            Y = 0x8000
        }

        internal GamePadState(bool isConnected, RawState rawState, GamePadDeadZone deadZone)
        {
            this.isConnected = isConnected;
            currentRawState = rawState;

            if (!isConnected)
            {
                rawState.DwPacketNumber = 0;
                rawState.Gamepad.DwButtons = 0;
                rawState.Gamepad.BLeftTrigger = 0;
                rawState.Gamepad.BRightTrigger = 0;
                rawState.Gamepad.SThumbLx = 0;
                rawState.Gamepad.SThumbLy = 0;
                rawState.Gamepad.SThumbRx = 0;
                rawState.Gamepad.SThumbRy = 0;
            }

            packetNumber = rawState.DwPacketNumber;
            buttons = new GamePadButtons(
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.Start) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.Back) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.LeftThumb) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.RightThumb) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.LeftShoulder) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.RightShoulder) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.Guide) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.A) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.B) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.X) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.Y) != 0 ? ButtonState.Pressed : ButtonState.Released
            );
            dPad = new GamePadDPad(
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.DPadUp) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.DPadDown) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.DPadLeft) != 0 ? ButtonState.Pressed : ButtonState.Released,
                (rawState.Gamepad.DwButtons & (uint)ButtonsConstants.DPadRight) != 0 ? ButtonState.Pressed : ButtonState.Released
            );

            switch (deadZone)
            {
                case GamePadDeadZone.IndependentAxes:
                    rawState.Gamepad.SThumbLx = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.SThumbLx, (short)Imports.Constants.LeftStickDeadZone);
                    rawState.Gamepad.SThumbLy = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.SThumbLy, (short)Imports.Constants.LeftStickDeadZone);
                    rawState.Gamepad.SThumbRx = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.SThumbRx, (short)Imports.Constants.RightStickDeadZone);
                    rawState.Gamepad.SThumbRy = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.SThumbRy, (short)Imports.Constants.RightStickDeadZone);
                    break;
            }

            thumbSticks = new GamePadThumbSticks(
                new GamePadThumbSticks.StickValue(
                    rawState.Gamepad.SThumbLx < 0 ? rawState.Gamepad.SThumbLx / 32768.0f : rawState.Gamepad.SThumbLx / 32767.0f,
                    rawState.Gamepad.SThumbLy < 0 ? rawState.Gamepad.SThumbLy / 32768.0f : rawState.Gamepad.SThumbLy / 32767.0f),
                new GamePadThumbSticks.StickValue(
                    rawState.Gamepad.SThumbRx < 0 ? rawState.Gamepad.SThumbRx / 32768.0f : rawState.Gamepad.SThumbRx / 32767.0f,
                    rawState.Gamepad.SThumbRy < 0 ? rawState.Gamepad.SThumbRy / 32768.0f : rawState.Gamepad.SThumbRy / 32767.0f)
            );
            triggers = new GamePadTriggers(rawState.Gamepad.BLeftTrigger / 255.0f, rawState.Gamepad.BRightTrigger / 255.0f);
        }

        public static short ThumbStickDeadZoneIndependantAxes(short value, short deadZone)
        {
            if (value < 0 && value > -deadZone || value > 0 && value < deadZone)
                return 0;
            return value;
        }

        public uint PacketNumber
        {
            get { return packetNumber; }
        }

        public bool IsConnected
        {
            get { return isConnected; }
        }

        public GamePadButtons Buttons
        {
            get { return buttons; }
        }

        public GamePadDPad DPad
        {
            get { return dPad; }
        }

        public GamePadTriggers Triggers
        {
            get { return triggers; }
        }

        public GamePadThumbSticks ThumbSticks
        {
            get { return thumbSticks; }
        }

        public RawState RawSate
        {
            get { return currentRawState; }
        }
    }

    public enum PlayerIndex
    {
        One = 0,
        Two,
        Three,
        Four
    }

    public enum GamePadDeadZone
    {
        IndependentAxes,
        None
    }

    public class GamePad
    {
        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            return GetState(playerIndex, GamePadDeadZone.IndependentAxes);
        }

        public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZone)
        {
            IntPtr gamePadStatePointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GamePadState.RawState)));
            uint result = Imports.XInputGetState((uint)playerIndex, gamePadStatePointer);
            GamePadState.RawState state = (GamePadState.RawState)Marshal.PtrToStructure(gamePadStatePointer, typeof(GamePadState.RawState));
            return new GamePadState(result == (uint)Imports.Constants.Success, state, deadZone);
        }

        public static void SetState(PlayerIndex playerIndex, double leftMotor, double rightMotor)
        {
            XInputVibration vibration = new XInputVibration() { LeftMotorSpeed = (ushort)(65535d * leftMotor), RightMotorSpeed = (ushort)(65535d * rightMotor) };
            Imports.XInputSetState((int)playerIndex, ref vibration);
        }

        public static string GetBatteryType(PlayerIndex playerIndex)
        {
            var gamepad = new XInputBatteryInformation();
            Imports.XInputGetBatteryInformation((int)playerIndex, 0, ref gamepad);

            return gamepad.ToString();
        }
    }
}
