using System;
using System.CodeDom;
using System.IO;
using System.Threading;
using System.Xml;
using consoleXstreamX.Capture;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;
using consoleXstreamX.Output;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectLoadProfile
    {
        private static bool _useTitanOne;
        private static bool _changeCaptureDevice;
        private static bool _changeCrossbar;
        private static string _setCrossbarVideo;
        private static string _setCrossbarAudio;
        public static void Show()
        {
            if (!Directory.Exists("Profiles")) Directory.CreateDirectory("Profiles");

            MenuActions.SetMenu("Load Profile");
            MenuActions.ClearSubMenu();

            if (File.Exists(@"Profiles\ps3.xml")) Shutter.AddItem("PlayStation 3", "ps3");
            if (File.Exists(@"Profiles\ps4.xml")) Shutter.AddItem("PlayStation 4", "ps4");
            if (File.Exists(@"Profiles\x360.xml")) Shutter.AddItem("Xbox 360", "x360");
            if (File.Exists(@"Profiles\xOne.xml")) Shutter.AddItem("Xbox One", "xOne");

            if (Shutter.Tiles.Count == 0)
            {
                Shutter.Error = "No profiles found";
                Shutter.Explanation = "Please create a profile first";
            }
            else

                Shutter.Selected = Shutter.Tiles[0].Command;

            Shutter.SetActiveRow(1);
        }

        public static void Load(string file)
        {
            _useTitanOne = false;
            _changeCaptureDevice = false;
            _changeCrossbar = false;

            if (!File.Exists($@"Profiles\{file}.xml")) return;

            var value = "";
            var reader = new XmlTextReader($@"Profiles\{file}.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        value = reader.Value;
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        if (!string.IsNullOrEmpty(value))
                        {
                            SetValue(reader.Name, value);
                        }
                        break;
                }
            }
            reader.Close();

            if (_useTitanOne)
            {
                _useTitanOne = false;
                TitanOne.Set();
            }

            if (_changeCaptureDevice && _changeCrossbar)
            {
                Settings.CaptureDevice = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].Title;
                VideoCapture.RunGraph();
                var b = 1;
                //need to fix this one
                /*
                var captureDevice = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
                captureDevice.CrossbarAudio = _setCrossbarAudio;
                captureDevice.CrossbarVideo = _setCrossbarVideo;
                Settings.SaveConfiguration();
                VideoCapture.CloseGraph();
                VideoCapture.ClearGraph();
                */
                return;
            }

            if (_changeCaptureDevice)
            {
                Settings.CaptureDevice = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].Title;
                Settings.SaveConfiguration();
                VideoCapture.RunGraph();
                return;
            }

            if (!_changeCrossbar) return;
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            device.CrossbarAudio = _setCrossbarAudio;
            device.CrossbarVideo = _setCrossbarVideo;
            Settings.SaveConfiguration();

            VideoCapture.ChangeCrossbarConnection();

            if (VideoCapture.MediaControl != null)
                VideoCapture.MediaControl.Run();
            else
                VideoCapture.RunGraph();
        }

        private static void SetValue(string title, string value)
        {
            if (string.Equals(title, "CaptureDevice", StringComparison.CurrentCultureIgnoreCase)) SetCaptureDevice(value);
            if (string.Equals(title, "CaptureAudio", StringComparison.CurrentCultureIgnoreCase)) SetCaptureAudio(value);
            if (string.Equals(title, "CrossbarVideo", StringComparison.CurrentCultureIgnoreCase)) SetCrossbarVideo(value);
            if (string.Equals(title, "CrossbarAudio", StringComparison.CurrentCultureIgnoreCase)) SetCrossbarAudio(value);

            if (string.Equals(title, "UseCronusMaxPlus", StringComparison.CurrentCultureIgnoreCase) && 
                string.Equals(value, "true", StringComparison.CurrentCultureIgnoreCase)) CronusmaxPlus.Set();

            if (string.Equals(title, "UseTitanOne", StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(value, "true", StringComparison.CurrentCultureIgnoreCase)) _useTitanOne = true;

            if (string.Equals(title, "TitanOneId", StringComparison.CurrentCultureIgnoreCase) &&
                !string.IsNullOrEmpty(value)) SetTitanOneDevice(value);

        }

        private static void SetTitanOneDevice(string value)
        {
            _useTitanOne = false;
            TitanOne.SetBySerial(value);
        }

        private static void SetCaptureDevice(string title)
        {
            var index = VideoCapture.CaptureDevices.FindIndex(s => String.Equals(s.Title, title, StringComparison.CurrentCultureIgnoreCase));
            if (index == -1) return;
            if (VideoCapture.CurrentVideoDevice != index) _changeCaptureDevice = true;
            VideoCapture.CurrentVideoDevice = index;
        }

        private static void SetCaptureAudio(string title)
        {
            var index = VideoCapture.AudioDevices.FindIndex(s => s.Equals(title, StringComparison.CurrentCultureIgnoreCase));
            if (index == -1) return;
            if (VideoCapture.CurrentAudioDevice != index) _changeCaptureDevice = true;
            VideoCapture.CurrentAudioDevice = index;
        }

        private static void SetCrossbarVideo(string value)
        {
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            if (device.CrossbarVideo == null) device.CrossbarVideo = device.VideoInput;
            if (!string.Equals(device.CrossbarVideo, value, StringComparison.CurrentCultureIgnoreCase)) _changeCrossbar = true;
            _setCrossbarVideo = value;
            Settings.CrossbarVideo = value;

            //VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].CrossbarVideo = value;
        }

        private static void SetCrossbarAudio(string value)
        {
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            if (device.CrossbarAudio == null) device.CrossbarAudio = device.AudioInput;
            if (!string.Equals(device.CrossbarAudio, value, StringComparison.CurrentCultureIgnoreCase)) _changeCrossbar = true;
            _setCrossbarAudio = value;
            Settings.CrossbarAudio = value;

            //VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].CrossbarAudio = value;
        }
    }
}
