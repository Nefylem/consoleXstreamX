using System.IO;
using System.Xml;
using consoleXstreamX.Capture;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectSaveProfile
    {
        public static void Show()
        {
            MenuActions.SetMenu("Save Profile");
            MenuActions.ClearSubMenu();

            Shutter.AddItem("PlayStation 3", "ps3");
            Shutter.AddItem("PlayStation 4", "ps4");
            Shutter.AddItem("Xbox 360", "x360");
            Shutter.AddItem("Xbox One", "xOne");

            Shutter.Selected = "ps3";
            Shutter.SetActiveRow(1);
        }

        public static void Save(string command)
        {
            if (!Directory.Exists("Profiles")) Directory.CreateDirectory("Profiles");
            var xmlWriterSettings = new XmlWriterSettings()
            {
                NewLineOnAttributes = true,
                Indent = true
            };

            using (var writer = XmlWriter.Create($@"Profiles\{command}.xml", xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Profile");

                writer.WriteStartElement("VideoCapture");
                writer.WriteElementString("CaptureDevice", Settings.CaptureDevice);
                writer.WriteElementString("CaptureAudio", Settings.CaptureAudio);
                writer.WriteElementString("CrossbarVideo", VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].VideoInput);
                writer.WriteElementString("CrossbarAudio", VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].AudioInput);
                writer.WriteEndElement();

                writer.WriteStartElement("Controller");
                writer.WriteElementString("UseCronusMaxPlus", Settings.UseCronusMaxPlus.ToString());
                writer.WriteElementString("UseTitanOne", Settings.UseTitanOne.ToString());
                if (Settings.UseTitanOne)
                {
                    writer.WriteElementString("UseTitanDevice", Settings.UseTitanDevice.ToString());
                    writer.WriteElementString("TitanOneId", Settings.TitanOneId);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            ShutterCommand.StepBackMenu();
        }
    }
}
