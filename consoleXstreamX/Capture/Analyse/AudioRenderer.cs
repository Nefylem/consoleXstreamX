using System.Collections.Generic;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.Analyse
{
    class AudioRenderer
    {
        public void Find()
        {
            VideoCapture.AudioDevices = new List<string>();
            Debug.Log("[0] Find audio devices");
            var audio = VideoCapture.AudioDevices;

            var devObject = DsDevice.GetDevicesOfCat(FilterCategory.AudioRendererCategory);
            foreach (var obj in devObject)
            {
                audio.Add(obj.Name);
                Debug.Log($"[4] Found audio device: {obj.Name}");
            }

            Debug.Log("");
        }
    }
}
