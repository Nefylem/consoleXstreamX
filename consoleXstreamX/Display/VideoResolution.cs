using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace consoleXstreamX.Resolution
{
    internal static class VideoResolution
    {
        public static void Set(Resolution newResolution, int graphicsCardId)
        {
            var currentDisplay = GetDevmode(graphicsCardId, -1);
            newResolution.Refresh = currentDisplay.dmDisplayFrequency;
            SetDisplay(newResolution, graphicsCardId);
        }

        private static void SetDisplay(Resolution resolution, int graphicsCardId)
        {
            var availableResolution = List(graphicsCardId);
            var index = availableResolution.FindIndex(s => s.Height == resolution.Height && s.Width == resolution.Width);
            if (index == -1) return;
            var set = GetDevmode(graphicsCardId, index);
            if (set.dmBitsPerPel == 0 && set.dmPelsWidth == 0 && set.dmPelsHeight == 0) return;
            ChangeDisplaySettings(ref set, 0);
        }

        public static List<Resolution> List(int id)
        {
            var results = new List<Resolution>();
            var devName = GetDeviceName(id);
            var devMode = new Devmode();
            var modeNum = 0;
            bool result;

            do
            {
                result = EnumDisplaySettings(devName, modeNum, ref devMode);
                if (result)
                {
                    var current = new Resolution()
                    {
                        Width = devMode.dmPelsWidth,
                        Height = devMode.dmPelsHeight
                    };

                    if (results.IndexOf(current) == -1) results.Add(current);
                }
                modeNum++;
            } while (result);

            return results;
        } 

        public static Resolution Get(int id)
        {
            var current = GetDevmode(id, -1);
            return new Resolution()
            {
                Width = current.dmPelsWidth,
                Height = current.dmPelsHeight,
                Refresh = current.dmDisplayFrequency
            };
        }

        private static Devmode GetDevmode(int devNum, int modeNum)
        {
            var devMode = new Devmode();
            var devName = GetDeviceName(devNum);
            EnumDisplaySettings(devName, modeNum, ref devMode);
            return devMode;
        }

        private static string GetDeviceName(int devNum)
        {
            var d = new DisplayDevice(0);
            var result = EnumDisplayDevices(IntPtr.Zero,
                devNum, ref d, 0);
            return (result ? d.DeviceName.Trim() : "#error#");
        }


        [DllImport("User32.dll")]
        private static extern bool EnumDisplayDevices(IntPtr lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);

        [DllImport("User32.dll")]
        private static extern bool EnumDisplaySettings(string devName, int modeNum, ref Devmode devMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref Devmode devMode, int flags);

        [StructLayout(LayoutKind.Sequential)]
        public struct DisplayDevice
        {
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;

            public DisplayDevice(int flags)
            {
                cb = 0;
                StateFlags = flags;
                DeviceName = new string((char)32, 32);
                DeviceString = new string((char)32, 128);
                DeviceID = new string((char)32, 128);
                DeviceKey = new string((char)32, 128);
                cb = Marshal.SizeOf(this);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Devmode
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public short dmOrientation;
            public short dmPaperSize;
            public short dmPaperLength;
            public short dmPaperWidth;
            public short dmScale;
            public short dmCopies;
            public short dmDefaultSource;
            public short dmPrintQuality;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmUnusedPadding;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
        }

        public class Resolution
        {
            public int Width;
            public int Height;
            public int Refresh;
        }

    }
}
