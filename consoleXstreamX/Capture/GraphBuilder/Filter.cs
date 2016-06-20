using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Filter
    {
        public IBaseFilter Set(Guid category, string title, out string temp)
        {
            var categoryTitle = "";

            if (category == FilterCategory.VideoInputDevice) { categoryTitle = "Video Capture Device"; }
            if (category == FilterCategory.AudioRendererCategory) { categoryTitle = "Audio Render Device"; }
            Debug.Log("");
            Debug.Log("[2]  Create " + title + " (" + categoryTitle + ")");

            var pFilter = Create(category, title, out temp);
            var hr = VideoCapture.CaptureGraph.AddFilter(pFilter, temp);
            if (hr == 0)
            {
                Debug.Log("[2] [OK] Added " + title + " (" + categoryTitle + ") to graph");
            }
            else
            {
                Debug.Log("[FAIL] Can't add " + title + " (" + categoryTitle + ") to graph");
                Debug.Log("-> " + DsError.GetErrorText(hr));
            }

            return pFilter;
        }

        public IBaseFilter Create(Guid category, string name, out string title)
        {
            Debug.Log("[3] Create filter: name>" + name);
            IBaseFilter filter = null;
            title = "*NF*";

            var devId = VideoCapture.CurrentActiveDevice;
            if (name.IndexOf('(') > -1)
            {
                var id = name.Substring(name.IndexOf('(') + 1);
                if (id.IndexOf(')') > -1) { id = id.Substring(0, id.IndexOf(')')); }
                name = name.Substring(0, name.IndexOf('(')).Trim();

                Debug.Log($"FilterName: {name}");
                Debug.Log($"DevID: {devId}");
                if (IsNumber(id)) devId = int.Parse(id);
                Debug.Log($"Confirm DevID: {devId}");
            }

            var deviceId = 0;
            foreach (DsDevice devObject in DsDevice.GetDevicesOfCat(category))
            {
                Debug.Log($"device> {devObject.Name}");
                if (devObject.Name.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) <= -1 && !String.Equals(devObject.Name, "oem crossbar", StringComparison.CurrentCultureIgnoreCase)) continue;

                Debug.Log($"TargetID={devId} [] DevID={deviceId}");

                if (devId == deviceId)
                {
                    VideoCapture.CurrentActiveDevice = deviceId;
                    Debug.Log($"SetDeviceID ({deviceId})");
                    title = devObject.Name;
                    IBindCtx bindCtx = null;
                    try
                    {
                        var hr = CreateBindCtx(0, out bindCtx);
                        DsError.ThrowExceptionForHR(hr);
                        var guid = typeof(IBaseFilter).GUID;
                        object obj;
                        devObject.Mon.BindToObject(bindCtx, null, ref guid, out obj);
                        filter = (IBaseFilter) obj;
                    }
                    finally
                    {
                        if (bindCtx != null) Marshal.ReleaseComObject(bindCtx);
                    }
                }
                else
                {
                    Debug.Log($"{deviceId} skipped");
                }
                deviceId++;
            }
            return filter;
        }

        private static bool IsNumber(string write)
        {
            return write.All(Char.IsDigit);
        }

        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);


    }
}
