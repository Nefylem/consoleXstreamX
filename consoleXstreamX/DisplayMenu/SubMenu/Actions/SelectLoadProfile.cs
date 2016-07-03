using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using consoleXstreamX.Capture;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectLoadProfile
    {
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

        }

        private static void SetValue(string title, string value)
        {
            if (string.Equals(title, "CaptureDevice", StringComparison.CurrentCultureIgnoreCase)) SetCaptureDevice(value);
            if (string.Equals(title, "CaptureAudio", StringComparison.CurrentCultureIgnoreCase)) SetCaptureAudio(value);
            if (string.Equals(title, "CrossbarVideo", StringComparison.CurrentCultureIgnoreCase)) VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].CrossbarVideo = value;
            if (string.Equals(title, "CrossbarAudio", StringComparison.CurrentCultureIgnoreCase)) VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].CrossbarAudio = value;

            VideoCapture.RunGraph();
        }

        private static void SetCaptureDevice(string title)
        {
            var index = VideoCapture.CaptureDevices.FindIndex(s => String.Equals(s.Title, title, StringComparison.CurrentCultureIgnoreCase));
            if (index == -1) return;
            VideoCapture.CurrentVideoDevice = index;
        }

        private static void SetCaptureAudio(string title)
        {
            var index = VideoCapture.AudioDevices.FindIndex(s => s.Equals(title, StringComparison.CurrentCultureIgnoreCase));
            if (index == -1) return;
            VideoCapture.CurrentActiveDevice = index;
        }
    }
}
