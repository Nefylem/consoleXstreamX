using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Capture.Settings;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class PrimaryDevice
    {
        public void Create()
        {
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            if (device.CurrentResolution > device.Resolution.Count) device.CurrentResolution = 0;

            if (VideoCapture.CurrentAudioDevice > VideoCapture.AudioDevices.Count) new UserSettings().SetAudio();

            VideoCapture.CurrentVideo = device.Title;
            VideoCapture.CurrentVideoShort = FindCaptureName(device.Title);
            VideoCapture.CurrentAudio = VideoCapture.AudioDevices[VideoCapture.CurrentAudioDevice];
            VideoCapture.CurrentResolution = device.Resolution[device.CurrentResolution].GetRes();

            Debug.Log($"[2] Video Capture Device: {VideoCapture.CurrentVideo}");
            Debug.Log($"[2] Video Capture Device ID: {VideoCapture.CurrentVideoShort}");
            Debug.Log($"[2] Resolution: {VideoCapture.CurrentResolution})");
            Debug.Log($"[2] Audio Renderer: {VideoCapture.CurrentAudio}");
        }

        /*
         * Looks for a unique identifier (eg Avermedia U3 extremecap will return Avermedia)
         * If there's more than one avermedia card, will look for the next available unique name
         * Finally, if there's two avermedia u3 on there, will still only return Avermedia
         */
        private string FindCaptureName(string device)
        {
            var output = "";
            foreach (var t in device)
            {
                if (t == ' ')
                {
                    if (FindCaptureResults(output) == 1) break;
                    output += t;
                }
                else output += t;
            }

            return output;
        }

        private static int FindCaptureResults(string name)
        {
            var result = 0;
            var listDev = new List<string>();

            foreach (var devName in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).Select(device => device.Name.ToLower()).Where(devName => listDev.IndexOf(devName) == -1))
            {
                listDev.Add(devName);

                if (devName.Length <= name.Length) continue;
                if (devName.Substring(0, name.Length) == name.ToLower())
                    result++;
            }

            listDev.Clear();

            return result;
        }
    }
}
