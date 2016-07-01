using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using consoleXstreamX.Configuration;
using consoleXstreamX.Debugging;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Input;

namespace consoleXstreamX.Output
{
    internal static class CronusmaxPlus
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);


        public struct GcapiConstants
        {
            public const int GcapiInputTotal = 30;
            public const int GcapiOutputTotal = 36;
        }
        public struct GcapiStatus
        {
            public byte Value; // Current value - Range: [-100 ~ 100] %
            public byte PrevValue; // Previous value - Range: [-100 ~ 100] %
            public int PressTv; // Time marker for the button press event
        }
        public struct Report
        {
            public byte Console; // Receives values established by the #defines CONSOLE_*
            public byte Controller; // Values from #defines CONTROLLER_* and EXTENSION_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Led; // Four LED - #defines LED_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] Rumble; // Two rumbles - Range: [0 ~ 100] %
            public byte BatteryLevel; // Battery level - Range: [0 ~ 10] 0 = empty, 10 = full

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GcapiConstants.GcapiInputTotal, ArraySubType = UnmanagedType.Struct)]
            public GcapiStatus[] Input;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte GcapiLoadPtr();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate byte GcapiIsconnectedPtr();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint GcapiGettimevalPtr();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint GcapiGetfwverPtr();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate byte GcapiWritePtr(byte[] output);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate byte GcapiWriteExPtr(byte[] output);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate byte GcapiWriterefPtr(byte[] output);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int GcapiCalcpresstimePtr(byte time);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void GcapiUnloadPtr();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GcapiReadCmPtr([In, Out] ref Report report);

        public static GcapiLoadPtr Load;
        public static GcapiIsconnectedPtr Connected;
        public static GcapiGettimevalPtr TimeVal;
        public static GcapiGetfwverPtr FwVer;
        public static GcapiWritePtr Write;
        public static GcapiWriteExPtr WriteEx;
        public static GcapiWriterefPtr WriteRef;
        public static GcapiReadCmPtr Read;
        public static GcapiCalcpresstimePtr PressTime;
        public static GcapiUnloadPtr Unload;

        private static bool _notConnected;
        private static bool _connected;
        private static bool _notCreated;

        public static void Open()
        {
            Debug.Log("[0] Opening ControllerMax api");
            var file = FindDll();
            if (string.IsNullOrEmpty(file))
            {
                Debug.Log("Error loading CronusmaxPlus gcdapi.dll");
                return;
            }

            var dll = LoadLibrary(file);
            if (dll == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] Unable to allocate Device API");
                return;
            }

            var load = LoadExternalFunction(dll, "gcdapi_Load");
            if (load == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcapi_Load"); return; }

            var connected = LoadExternalFunction(dll, "gcapi_IsConnected");
            if (connected == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcapi_IsConnected"); return; }

            var unload = LoadExternalFunction(dll, "gcdapi_Unload");
            if (unload == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcapi_Unload"); return; }

            var timeVal = LoadExternalFunction(dll, "gcapi_GetTimeVal");
            if (timeVal == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcapi_GetTimeVal"); return; }

            var fwVer = LoadExternalFunction(dll, "gcapi_GetFWVer");
            if (fwVer == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcapi_GetFWVer"); return; }

            var write = LoadExternalFunction(dll, "gcapi_Write");
            if (write == IntPtr.Zero) return;

            var read = LoadExternalFunction(dll, "gcapi_Read");
            if (read == IntPtr.Zero) return;

            var pressTime = LoadExternalFunction(dll, "gcapi_CalcPressTime");
            if (pressTime == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcapi_CalcPressTime"); return; }

            try
            {
                Load = (GcapiLoadPtr)Marshal.GetDelegateForFunctionPointer(load, typeof(GcapiLoadPtr));
                Connected = (GcapiIsconnectedPtr)Marshal.GetDelegateForFunctionPointer(connected, typeof(GcapiIsconnectedPtr));
                Unload = (GcapiUnloadPtr)Marshal.GetDelegateForFunctionPointer(unload, typeof(GcapiUnloadPtr));
                TimeVal = (GcapiGettimevalPtr)Marshal.GetDelegateForFunctionPointer(timeVal, typeof(GcapiGettimevalPtr));
                FwVer = (GcapiGetfwverPtr)Marshal.GetDelegateForFunctionPointer(fwVer, typeof(GcapiGetfwverPtr));
                Write = (GcapiWritePtr)Marshal.GetDelegateForFunctionPointer(write, typeof(GcapiWritePtr));
                Read = (GcapiReadCmPtr)Marshal.GetDelegateForFunctionPointer(read, typeof(GcapiReadCmPtr));
                PressTime = (GcapiCalcpresstimePtr)Marshal.GetDelegateForFunctionPointer(pressTime, typeof(GcapiCalcpresstimePtr));
            }
            catch (Exception ex)
            {
                Debug.Log("[0] Fail -> " + ex);
            }


            Load();
            Debug.Log("[0] Initialize ControllerMax API ok");
        }


        private static string FindDll()
        {
            var files = new List<string>()
            {
                "gcdapi.dll",
                "cronusmaxPlus_gcdapi.dll",
                @"CronusmaxPlus\gcdapi.dll",
                @"Data\cronusmaxPlus_gcdapi.dll",
                @"Data\gcdapi.dll",
                @"Data\CronusmaxPlus\gcdapi.dll"
            };

            foreach (var item in files)
            {
                if (File.Exists(item)) return item;
            }
            return "";
        }

        private static IntPtr LoadExternalFunction(IntPtr dll, string function)
        {
            var pointer = GetProcAddress(dll, function);
            Debug.Log(pointer == IntPtr.Zero ? $"[0] [NG] {function} alloc fail" : $"[5] [OK] {function}");
            return pointer;
        }

        public static void Send(Gamepad.GamepadOutput player)
        {
            if (Write == null)
            {
                if (!_notCreated)
                {
                    _notCreated = true;
                    Debug.Log("Device API not properly created");
                }
                return;
            }
            if (MenuController.Visible) return;
            if (Connected() != 1)
            {
                if (!_notConnected)
                {
                    _notConnected = true;
                    _connected = false;
                    Debug.Log("Device not connected");
                }
                return;
            }

            if (!_connected)
            {
                _connected = true;
                _notConnected = false;
                Debug.Log("Device connected");
            }

            Write(player.Output);
            var report = new Report();
            if (Read(ref report) == IntPtr.Zero) return;
            if (Settings.Rumble) Gamepad.SetState(player.Index, report.Rumble[0], report.Rumble[1]);
        }

        public static void Close()
        {
            Unload?.Invoke();

            Load = null;
            Connected = null;
            TimeVal = null;
            FwVer = null;
            Write = null;
            WriteEx = null;
            WriteRef = null;
            Read = null;
            PressTime = null;
            Unload = null;

            Debug.Log("[OK] Closed ControllerMax API");
        }

    }
}
