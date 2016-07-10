using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using consoleXstreamX.Configuration;
using consoleXstreamX.Debugging;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Input;
using consoleXstreamX.Remapping;

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

        public struct Report
        {
            public byte Console; // Receives values established by the #defines CONSOLE_*
            public byte Controller; // Values from #defines CONTROLLER_* and EXTENSION_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] Led; // Four LED - #defines LED_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] //XBOX ONE TRIGGER RUMBLE
            public byte[] Rumble; // Two rumbles - Range: [0 ~ 100] %

            public byte BatteryLevel; // Battery level - Range: [0 ~ 10] 0 = empty, 10 = full

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GcmapiConstants.GcapiInputTotal,
                ArraySubType = UnmanagedType.Struct)] public GcmapiStatus[] Input;
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
        public delegate IntPtr GcmapiRead(int device, [In, Out] ref Report report);

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

        private static List<int> _notConnected;
        private static List<int> _connected; 

        public static List<TitanDevices> DeviceList;

        public static void Open()
        {
            var file = FindDll();
            if (String.IsNullOrEmpty(file))
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
            if (load == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] gcMapi_Load");
                return;
            }

            var unload = LoadExternalFunction(dll, "gcmapi_Unload");
            if (unload == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] gcMapi_Unload");
                return;
            }

            var connect = LoadExternalFunction(dll, "gcmapi_Connect");
            if (connect == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] gcmapi_Connect");
                return;
            }

            var connected = LoadExternalFunction(dll, "gcmapi_IsConnected");
            if (connected == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] gcmapi_IsConnected");
                return;
            }

            var serial = LoadExternalFunction(dll, "gcmapi_GetSerialNumber");
            if (serial == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] gcmapi_GetSerialNumber");
                return;
            }

            var write = LoadExternalFunction(dll, "gcmapi_Write");
            if (write == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] gcmapi_Write");
                return;
            }

            var read = LoadExternalFunction(dll, "gcmapi_Read");
            if (read == IntPtr.Zero)
            {
                Debug.Log("[0] [FAIL] gcmapi_Read");
                return;
            }

            try
            {
                Load = (GcmapiLoad) Marshal.GetDelegateForFunctionPointer(load, typeof(GcmapiLoad));
                Unload = (GcmapiUnload) Marshal.GetDelegateForFunctionPointer(unload, typeof(GcmapiUnload));
                Connect = (GcmapiConnect) Marshal.GetDelegateForFunctionPointer(connect, typeof(GcmapiConnect));
                Serial = (GcmapiGetserialnumber) Marshal.GetDelegateForFunctionPointer(serial, typeof(GcmapiGetserialnumber));
                Write = (GcmapiWrite) Marshal.GetDelegateForFunctionPointer(write, typeof(GcmapiWrite));
                Connected = (GcmapiIsconnected) Marshal.GetDelegateForFunctionPointer(connected, typeof(GcmapiIsconnected));
                Read = (GcmapiRead) Marshal.GetDelegateForFunctionPointer(read, typeof(GcmapiRead));
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

        public static List<TitanDevices> FindDevices()
        {
            var results = new List<TitanDevices>();

            if (Connect == null) return results;

            var deviceCount = Load();
            Debug.Log($"Number of devices found: {deviceCount}");
            Connect((ushort) DevPid.TitanOne);

            Thread.Sleep(10);

            for (var count = 0; count <= deviceCount; count++)
            {
                if (Connected(count) == 0) continue;
                var serial = ReadSerial(count);

                Debug.Log($"Device found: [ID]{count} [SERIAL]{serial}");

                results.Add(new TitanDevices()
                {
                    Id = count,
                    SerialNo = serial
                });
            }

            if (!String.IsNullOrEmpty(Settings.TitanOneId))
            {
                var record = results.FirstOrDefault(s => s.SerialNo == Settings.TitanOneId);
                if (record != null) Settings.UseTitanDevice = record.Id;
            }

            DeviceList = results;
            return results;
        }

        private static string ReadSerial(int devId)
        {
            var serial = new byte[20];
            var ret = Serial(devId);
            Marshal.Copy(ret, serial, 0, 20);
            var serialNo = "";
            foreach (var item in serial)
            {
                serialNo += $"{item:X2}";
            }
            return serialNo;
        }

        public static void Send(Gamepad.GamepadOutput player)
        {
            if (MenuController.Visible) return;
            if (_notConnected == null) _notConnected = new List<int>();
            if (_connected == null) _connected = new List<int>();

            if (Connected(Settings.UseTitanDevice) != 1)
            {
                if (_notConnected.IndexOf(Settings.UseTitanDevice) > -1) return;
                if (_connected.IndexOf(Settings.UseTitanDevice) > -1) _connected.Remove(Settings.UseTitanDevice);
                _notConnected.Add(Settings.UseTitanDevice);
                Debug.Log($"TitanOne device {Settings.UseTitanDevice} not connected");
                return;
            }

            if (_connected.IndexOf(Settings.UseTitanDevice) == -1)
            {
                _connected.Add(Settings.UseTitanDevice);
                if (_notConnected.IndexOf(Settings.UseTitanDevice) > -1) _notConnected.Remove(Settings.UseTitanDevice);
                Debug.Log($"TitanOne device {Settings.UseTitanDevice} connected");
            }

            Write(Settings.UseTitanDevice, player.Output);

            if (!Settings.Rumble && !Settings.AllowPassthrough) return;
            var report = new Report();
            if (Read(Settings.UseTitanDevice, ref report) == IntPtr.Zero) return;
            if (Settings.AllowPassthrough) Gamepad.TitanOutput = report.Input;
            if (Settings.Rumble) Gamepad.SetState(player.Index, report.Rumble[0], report.Rumble[1]);
        }

        public static void Set(string command = "")
        {
            int devId;
            if (!int.TryParse(command, out devId)) devId = 0;

            Settings.UseTitanDevice = devId;
            Settings.TitanOneId = DeviceList.Count > devId ? DeviceList[devId].SerialNo : "";
            Settings.UseCronusMaxPlus = false;
            Settings.UseTitanOne = true;
            Settings.UseGimxRemote = false;
            Settings.SaveConfiguration();
        }

        public static void SetBySerial(string serial = "")
        {
            var record = DeviceList.FirstOrDefault(s => string.Equals(s.SerialNo, serial, StringComparison.CurrentCultureIgnoreCase));
            if (record != null)
            {
                Settings.UseTitanDevice = record.Id;
                Settings.TitanOneId = record.SerialNo;
            }
            else
            {
                var device = DeviceList.FirstOrDefault();
                if (device != null)
                {
                    Settings.UseTitanDevice = device.Id;
                    Settings.TitanOneId = device.SerialNo;
                }
                else
                {
                    Settings.UseTitanDevice = 0;
                    Settings.TitanOneId = "";
                }
            }

            Settings.UseTitanOne = true;
            Settings.UseCronusMaxPlus = false;
            Settings.UseGimxRemote = false;

            Settings.SaveConfiguration();
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

        public class TitanDevices
        {
            public int Id;
            public string SerialNo;
        }
    }
}