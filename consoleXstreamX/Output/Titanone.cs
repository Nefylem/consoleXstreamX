using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using consoleXstreamX.Debugging;

namespace consoleXstreamX.Output
{
    internal static class TitanOne
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        public struct GcmapiConstants
        {
            public const int GcapiInputTotal = 30;
            public const int GcapiOutputTotal = 36;
        }

        public struct GcmapiStatus
        {
            public byte Value; // Current value - Range: [-100 ~ 100] %
            public byte PrevValue; // Previous value - Range: [-100 ~ 100] %
            public int PressTv; // Time marker for the button press event
        }

        public struct GcmapiReport
        {
            public byte Console; // Receives values established by the #defines CONSOLE_*
            public byte Controller; // Values from #defines CONTROLLER_* and EXTENSION_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Led; // Four LED - #defines LED_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]        //XBOX ONE TRIGGER RUMBLE
            public byte[] Rumble; // Two rumbles - Range: [0 ~ 100] %
            public byte BatteryLevel; // Battery level - Range: [0 ~ 10] 0 = empty, 10 = full

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GcmapiConstants.GcapiInputTotal, ArraySubType = UnmanagedType.Struct)]
            public GcmapiStatus[] Input;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte GcmapiLoad();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void GcmapiUnload();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int GcmapiConnect(ushort devPid);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GcmapiGetserialnumber(int devId);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int GcmapiIsconnected(int m);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int GcmapiWrite(int device, byte[] output);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr GcmapiRead(int device, [In, Out] ref GcmapiReport gcapiReport);

        public enum DevPid
        {
            Any = 0x000,
            ControllerMax = 0x001,
            Cronus = 0x002,
            TitanOne = 0x003
        };

        public static GcmapiLoad Load;
        public static GcmapiUnload Unload;
        public static GcmapiConnect Connect;
        public static GcmapiGetserialnumber Serial;
        public static GcmapiIsconnected Connected;
        public static GcmapiWrite Write;
        public static GcmapiRead Read;

        public static void Open()
        {
            var file = FindDll();
            if (string.IsNullOrEmpty(file))
            {
                Debug.Log("Error loading TitanOne gcdapi.dll");
                return;
            }

            var dll = LoadLibrary(file);
            if (dll == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] Unable to allocate Device API");
                return;
            }

            var load = LoadExternalFunction(dll, "gcmapi_Load");
            if (load == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcMapi_Load"); return; }

            var unload = LoadExternalFunction(dll, "gcmapi_Unload");
            if (unload == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcMapi_Unload"); return; }

            var connect = LoadExternalFunction(dll, "gcmapi_Connect");
            if (connect == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcmapi_Connect"); return; }

            var connected = LoadExternalFunction(dll, "gcmapi_IsConnected");
            if (connected == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcmapi_IsConnected"); return; }

            var serial = LoadExternalFunction(dll, "gcmapi_GetSerialNumber");
            if (serial == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcmapi_GetSerialNumber"); return; }

            var write = LoadExternalFunction(dll, "gcmapi_Write");
            if (write == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcmapi_Write"); return; }

            var read = LoadExternalFunction(dll, "gcmapi_Read");
            if (read == IntPtr.Zero) { Debug.Log("[0] [FAIL] gcmapi_Read"); return; }

            try
            {
                Load = (GcmapiLoad)Marshal.GetDelegateForFunctionPointer(load, typeof(GcmapiLoad));
                Unload = (GcmapiUnload)Marshal.GetDelegateForFunctionPointer(unload, typeof(GcmapiUnload));
                Connect = (GcmapiConnect)Marshal.GetDelegateForFunctionPointer(connect, typeof(GcmapiConnect));
                Serial = (GcmapiGetserialnumber)Marshal.GetDelegateForFunctionPointer(serial, typeof(GcmapiGetserialnumber));
                Write = (GcmapiWrite)Marshal.GetDelegateForFunctionPointer(write, typeof(GcmapiWrite));
                Connected = (GcmapiIsconnected)Marshal.GetDelegateForFunctionPointer(connected, typeof(GcmapiIsconnected));
                Read = (GcmapiRead)Marshal.GetDelegateForFunctionPointer(read, typeof(GcmapiRead));
            }
            catch (Exception ex)
            {
                Debug.Log("[0] Fail -> " + ex);
                Debug.Log("[0] [ERR] Critical failure loading TitanOne API.");
                return;
            }

            //Dont load yet
            Debug.Log("TitanOne API initialised ok");
        }

        private static IntPtr LoadExternalFunction(IntPtr dll, string function)
        {
            var ptr = GetProcAddress(dll, function);
            Debug.Log(ptr == IntPtr.Zero ? $"[0] [NG] {function} alloc fail" : $"[5] [OK] {function}");
            return ptr;
        }

        private static string FindDll()
        {
            var files = new List<string>()
            {
                "gcdapi.dll",
                "titanOne_gcdapi.dll",
                @"TitanOne\gcdapi.dll",
                @"Data\titanOne_gcdapi.dll",
                @"Data\gcdapi.dll",
                @"Data\TitanOne\gcdapi.dll"
            };

            foreach (var item in files)
            {
                if (File.Exists(item)) return item;
            }
            return "";
        }

        public static void FindDevices()
        {
            if (Connect == null) return;
            var deviceCount = Load();
            Connect((ushort)DevPid.TitanOne);
            for (var count = 0; count <= deviceCount; count++)
            {
                if (Connected(count) == 0) continue;
                var serial = ReadSerial(count);
                var b = 1;
            }
            var c = 1;
            /*
             * var result = _deviceCount;
            for (var count = 0; count < _deviceCount; count++)
            {
                if (_class.MDefine.GcmapiIsConnected(count) == 0) 
                    result = 0;
            }

            _timeOut--;
            if (_timeOut == 0)
                return -1;

            if (result != 0)
            {
                //_class.System.Debug("listAll.log", "result : " + result + " now reading serials");
                ReadSerials();
            }

            return result;  
            */
            /*
            for (var count = 0; count < _deviceCount; count++)
            {
                var serial = new byte[20];
                var ret = _class.MDefine.GcmapiGetSerialNumber(count);
                Marshal.Copy(ret, serial, 0, 20);
                var disp = "";

                for (var counter = 0; counter < 20; counter++)
                {
                    disp += String.Format("{0:X2}", serial[counter]);
                }

                //Load config on each of these devices
                _class.MWrite.AddDevice(disp);

                if (_class.FrmMain.ListToDevices == null)
                    _class.FrmMain.ListToDevices = new List<string>();

                _class.FrmMain.ListToDevices.Add(disp);
            }

            if (_class.FrmMain.RetrySetTitanOne == null) return;
            if (_class.FrmMain.RetrySetTitanOne.Length <= 0) return;
            _class.MWrite.SetDevice(_class.FrmMain.RetrySetTitanOne);
            _class.FrmMain.RetrySetTitanOne = "";
            */
        }

        private static string ReadSerial(int devId)
        {
            var serial = new byte[20];
            var ret = Serial(devId);
            Marshal.Copy(ret, serial, 0, 20);
            var serialNo = "";
            foreach (var item in serial)
            {
                serialNo += string.Format("{0:X2}", item);
            }
            return serialNo;
        }

        public static void Close()
        {
            Unload?.Invoke();
            Connect = null;
            Serial = null;
            Connected = null;
            Load = null;
            Unload = null;
            Read = null;
            Debug.Log("[OK] Closed TitanOne API");
        }

    }
}
