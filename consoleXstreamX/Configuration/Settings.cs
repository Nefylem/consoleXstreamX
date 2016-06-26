using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace consoleXstreamX.Configuration
{
    internal static class Settings
    {
        public static bool InternalCapture;

        //Gamepad
        public static bool Rumble;
        public static bool Ps4ControllerMode;
        public static bool NormalizeControls;


        //CXS Interface controls
        public static bool BlockMenuCommand;
        public static bool UseShortcutKeys;

        //Mouse / keyboard interface
        public static bool HideMouse;
        public static bool EnableMouse;
        public static bool EnableKeyboard;

        //Display
        public static bool CheckFps;
        public static bool StayOnTop;
        public static bool AutoSetDisplayResolution;
        public static bool AutoSetCaptureResolution;
        public static string RefreshRate;
        public static string DisplayResolution;
        public static bool Fullscreen;

        //Output
        public static bool UseCronusMax;
        public static bool UseTitanOne;
        public static bool UseTitanOneApi;
        public static bool UseGimxRemote;
        public static string GimxAddress;
        public static int GimxKeepAlive;

        public static string CaptureProfile;

        public static string CurrentResolution;
        public static string SetResolution;

        //Debug
        public static string LogPath;
        public static int SystemDebugLevel;
        public static int DetailedLogs;

        static Settings()
        {
            AutoSetCaptureResolution = true;
            NormalizeControls = true;
            UseShortcutKeys = true;
            StayOnTop = true;
            CheckFps = true;

            SystemDebugLevel = 5;
            DetailedLogs = 0;
        }
    }
}
