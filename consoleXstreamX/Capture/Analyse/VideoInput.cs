using System.Collections.Generic;
using System.Linq;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.Analyse
{
    class VideoInput
    {
        public void Find()
        {
            VideoCapture.CaptureDevices = new List<VideoCapture.VideoCaptureDevices>();

            Debug.Log("[0] Listing video capture devices");
            var devObjects = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            if (devObjects.Length == 0)
            {
                Debug.Log("[Err] No capture devices found");
                return;
            }

            foreach (var obj in devObjects)
            {
                var title = obj.Name;
                var deviceId = 1;
                Debug.Log($"[4] Found capture device: {obj.Name}");

                while (VideoCapture.CaptureDevices.FirstOrDefault(s => s.Title == title) != null)
                {
                    title = $"{obj.Name} ({deviceId})";
                    deviceId++;
                }

                VideoCapture.CaptureDevices.Add(new VideoCapture.VideoCaptureDevices()
                {
                    Title = title,
                    Resolution = new List<VideoCapture.VideoCaptureResolution>(),
                });
            }
            Debug.Log("");
        }
    }
}
