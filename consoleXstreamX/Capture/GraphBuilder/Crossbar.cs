using System;
using System.Collections.Generic;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Crossbar
    {
        public CrossbarPins Create(string video, string audio, string shortName, IBaseFilter pCaptureDevice)
        {
            var results = new CrossbarPins();

            var pin = new Pin();
            string temp;

            Debug.Log("");
            Debug.Log($"[1] Looking for crossbar on device {VideoCapture.CurrentVideo} #{VideoCapture.CurrentActiveDevice}");

            var pCrossbar = new Filter().Create(FilterCategory.AMKSCrossbar, shortName, out temp);
            if (temp.ToLower() == "*nf*")
            {
                Debug.Log("[FAIL] No crossbar found. Will not interrupt operation");
                return results;
            }

            var hr = VideoCapture.CaptureGraph.AddFilter(pCrossbar, temp);
            if (hr == 0)
            {
                Debug.Log("[OK] Create crossbar");
                VideoCapture.ActiveCrossbar = true;
                results.Found = true;

                var pinList = new Pin().List(pCrossbar);
                results.Audio = pin.Assume("Audio", pinList.Out);
                results.Video = pin.Assume("Video", pinList.Out);
                Debug.Log("<Audio>" + results.Audio);
                Debug.Log("<Video>" + results.Video);

                Debug.Log("");
                Debug.Log("Connect Crossbar (" + results.Video + ") to Capture (" + video + ")");

                hr = VideoCapture.CaptureGraph.ConnectDirect(pin.Get(pCrossbar, results.Video), pin.Get(pCaptureDevice, video), null);
                Debug.Log("Crossbar Video -> " + DsError.GetErrorText(hr));

                hr = VideoCapture.CaptureGraph.ConnectDirect(pin.Get(pCrossbar, results.Audio), pin.Get(pCaptureDevice, audio), null);
                Debug.Log("Crossbar Audio -> " + DsError.GetErrorText(hr));

                VideoCapture.XBar = (IAMCrossbar)pCrossbar;
                Debug.Log("");

                return results;
            }

            Debug.Log("[FAIL] Can't add " + shortName + " Crossbar to graph");
            Debug.Log("-> " + DsError.GetErrorText(hr));
            Debug.Log("");
            results.Found = false;
            return results;
        }

        public static void Check()
        {
            if (VideoCapture.XBar == null) return;
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            if (!string.IsNullOrEmpty(device.CrossbarVideo) && !string.Equals(device.CrossbarVideo, "none"))
            {
                Set(device.CrossbarVideo, "Video");
            }

            if (!string.IsNullOrEmpty(device.CrossbarAudio) && !string.Equals(device.CrossbarAudio, "none"))
            {
                Set(device.CrossbarAudio, "Audio");
            }
        }

        private static void Set(string type, string description)
        {
            var changeXbar = Find(type, "");
            Debug.Log($"Change crossbar command ({description}): {changeXbar.Type} / {changeXbar.Pin}");
            Change(changeXbar);
        }

        private static CrossbarTarget Find(string type, string description)
        {
            var result = new CrossbarTarget();
            var outputs = ListOutputByType();
            if (outputs.Count == 0) return result;

            for (var count = 0; count < outputs.Video.Count; count++)
            {
                if (string.Equals(outputs.Video[count], type, StringComparison.CurrentCultureIgnoreCase))
                {
                    return new CrossbarTarget()
                    {
                        Type = 0,
                        Pin = count
                    };
                }
            }

            for (var count = 0; count < outputs.Audio.Count; count++)
            {
                if (string.Equals(outputs.Audio[count], type, StringComparison.CurrentCultureIgnoreCase))
                {
                    return new CrossbarTarget()
                    {
                        Type = 1,
                        Pin = outputs.Video.Count + count
                    };
                }
            }

            return result;
        }

        public static void Change(CrossbarTarget target)
        {
            if (VideoCapture.XBar == null) return;
            var hr = VideoCapture.XBar.Route(target.Type, target.Pin);
            if (hr != 0) { Debug.Log("[ERR] " + DsError.GetErrorText(hr)); }
        }

        public static List<string> ListOutputs()
        {
            var results = new List<string>();
            if (VideoCapture.XBar == null) return results;

            int inPin;
            int outPin;

            VideoCapture.XBar.get_PinCounts(out inPin, out outPin);
            for (var count = 0; count < outPin; count++)
            {
                int intRouted;
                int intPinId;
                PhysicalConnectorType pinType;

                VideoCapture.XBar.get_CrossbarPinInfo(true, count, out intPinId, out pinType);
                VideoCapture.XBar.get_IsRoutedTo(count, out intRouted);
                results.Add(pinType.ToString());
            }
            return results;
        }

        public static CrossbarItems ListOutputByType()
        {
            var results = new CrossbarItems
            {
                Video = new List<string>(),
                Audio = new List<string>()
            };

            if (VideoCapture.XBar == null) return results;

            int inPin;
            int outPin;

            VideoCapture.XBar.get_PinCounts(out inPin, out outPin);
            for (var count = 0; count < outPin; count++)
            {
                int intRouted;
                int intPinId;
                PhysicalConnectorType pinType;

                VideoCapture.XBar.get_CrossbarPinInfo(true, count, out intPinId, out pinType);
                VideoCapture.XBar.get_IsRoutedTo(count, out intRouted);
                var name = pinType.ToString();
                if (string.IsNullOrEmpty(name)) continue;
                if (name.IndexOf("Video_", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    results.Video.Add(name);
                    results.Count++;
                }
                if (name.IndexOf("Audio_", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    results.Audio.Add(name);
                    results.Count++;
                }
            }
            return results;
        }

        public static int GetActivePin(string type)
        {
            if (VideoCapture.XBar == null) return 0;
            int result;
            var connection = string.Equals(type, "video", StringComparison.CurrentCultureIgnoreCase) ? 0 : 1;
            VideoCapture.XBar.get_IsRoutedTo(connection, out result);
            return result;
        }

        public class CrossbarPins
        {
            public string Video;
            public string Audio;
            public bool Found;
        }

        public class CrossbarTarget
        {
            public int Type;
            public int Pin;
        }

        public class CrossbarItems
        {
            public List<string> Video;
            public List<string> Audio;
            public int Count;
        }
    }
}
