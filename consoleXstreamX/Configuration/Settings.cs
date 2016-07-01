using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace consoleXstreamX.Configuration
{
    public static class Settings
    {
        public static bool InternalCapture;

        //Gamepad
        public static bool Rumble;
        public static bool Ps4ControllerMode;
        public static bool NormalizeControls;


        //CXS Interface controls
        public static bool BlockMenuCommand;
        public static bool UseShortcutKeys;

        //Mouse / keyboard interface
        public static bool HideMouse;
        public static bool EnableMouse;
        public static bool EnableKeyboard;

        //Capture
        public static string CaptureDevice;
        public static string CaptureAudio;
        public static string CrossbarVideo;
        public static string CrossbarAudio;

        public static bool CheckFps;
        public static bool StayOnTop;
        public static bool AutoSetDisplayResolution;
        public static bool AutoSetCaptureResolution;
        public static bool Fullscreen;
        public static string RefreshRate;
        public static string DisplayResolution;
        public static string GraphicsCard;
        public static int GraphicsCardId;

        //Output
        public static bool UseCronusMaxPlus;
        public static bool UseTitanOne;
        public static bool UseTitanOneApi;
        public static bool UseGimxRemote;
        public static string GimxAddress;
        public static int GimxKeepAlive;

        public static string CaptureProfile;

        public static string CurrentResolution;
        public static string SetResolution;

        //Debug
        public static string LogPath;
        public static int SystemDebugLevel;
        public static int DetailedLogs;

        static Settings()
        {
            UseCronusMaxPlus = true;
            UseTitanOne = true;

            AutoSetDisplayResolution = false;
            AutoSetCaptureResolution = true;

            NormalizeControls = true;
            UseShortcutKeys = true;
            StayOnTop = true;
            CheckFps = true;

            SystemDebugLevel = 5;
            DetailedLogs = 0;
        }

        public static void SaveConfiguration()
        {
            var settings = typeof(Settings);
            var fields = settings.GetFields(BindingFlags.Static | BindingFlags.Public);

            var xmlWriterSettings = new XmlWriterSettings()
            {
                NewLineOnAttributes = true,
                Indent = true
            };
            using (XmlWriter writer = XmlWriter.Create("config.xml", xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Configuration");

                foreach (FieldInfo field in fields)
                {
                    var value = field.GetValue(null);
                    if (value == null) continue;
                    writer.WriteElementString(field.Name, value.ToString());
                };

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public static void LoadConfiguration()
        {
            var settings = typeof(Settings);
            var fields = settings.GetFields(BindingFlags.Static | BindingFlags.Public);

            var value = "";
            var reader = new XmlTextReader("config.xml");
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
                            var setField = fields.FirstOrDefault(s => s.Name.ToLower() == reader.Name.ToLower());
                            if (setField != null)
                            {
                                var setType = setField.FieldType.FullName;
                                if (string.Equals(setType, "System.Boolean", StringComparison.CurrentCultureIgnoreCase)) { setField.SetValue(null, GetBool(value)); continue;  }
                                if (string.Equals(setType, "System.Int32", StringComparison.CurrentCultureIgnoreCase)) { setField.SetValue(null, GetInt(value)); continue; }
                                if (string.Equals(setType, "System.String", StringComparison.CurrentCultureIgnoreCase)) { setField.SetValue(null, value); continue; }

                                var b = 1;

                            }
                        }
                        break;
                }
            }
            reader.Close();
        }

        private static bool GetBool(string write)
        {
            return string.Equals(write, "true", StringComparison.CurrentCultureIgnoreCase);
        }

        private static int GetInt(string write)
        {
            try
            {
                return int.Parse(write);
            }
            catch
            {
                return 0;
            }
        }
    }
}
