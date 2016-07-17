using System;
                                                                                                                                                using System.IO;
using System.Reflection;
using System.Xml;
using consoleXstreamX.Debugging;

namespace consoleXstreamX.PowerOn
{
    internal static class PowerStartup
    {
        /*
            This is the execution strategy for power on.
            User needs eventghost installed. Then either get them to manually register events, or pass in automatically ( would prefer to auto configure ).
            Add a config to tell cxs what profile name = what console
            Then simply use FireEvent to trigger the eventghost Profile
        */
        public static void Create()
        {
            if (!Directory.Exists("PowerProfiles")) Directory.CreateDirectory("PowerProfiles");
            CreatePs3();
            CreateXbox360();
            CreateXboxOne();
            CreatePowerProfile();
        }

        private static void CreatePowerProfile()
        {
            if (File.Exists(@"PowerProfiles\powerOn.xml")) return;

            var xmlWriterSettings = new XmlWriterSettings()
            {
                NewLineOnAttributes = true,
                Indent = true
            };
            using (XmlWriter writer = XmlWriter.Create(@"PowerProfiles\powerOn.xml", xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Profiles");
                    writer.WriteStartElement("Console");
                    writer.WriteElementString("EventGhost", "ps3");
                    writer.WriteElementString("Display", "Playstation 3");
                    writer.WriteEndElement();

                    writer.WriteStartElement("Console");
                    writer.WriteElementString("EventGhost", "x360");    
                    writer.WriteElementString("Display", "Xbox 360");
                    writer.WriteEndElement();

                    writer.WriteStartElement("Console");
                    writer.WriteElementString("EventGhost", "xOne");
                    writer.WriteElementString("Display", "Xbox One");
                    writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private static void CreatePs3()
        {
            if (File.Exists(@"PowerProfiles\ps3.txt")) return;
            var txtOut = new StreamWriter(@"PowerProfiles\ps3.txt");
            txtOut.WriteLine("You need to create an EventGhost IR Event. Enter this code");
            txtOut.WriteLine("0000 006C 0000 001A 0099 00AD 000F 0027 0010 0027 000F 0013 000F 0027 0010 0013 000F 0013 000F 0027 0010 0013 000F 0013 000F 0027 0010 0027 000F 0027 000F 0027 0010 0027 000F 0027 000F 0013 0010 0027 000F 0013 000F 0013 0010 0013 000F 0013 000F 0013 0010 0013 000F 0027 000F 077F");
            txtOut.Close();
        }

        private static void CreateXbox360()
        {
            if (File.Exists(@"PowerProfiles\xbox360.txt")) return;
            var txtOut = new StreamWriter(@"PowerProfiles\xbox360.txt");
            txtOut.WriteLine("You need to create an EventGhost IR Event. Enter this code");
            txtOut.WriteLine("0000 0073 0000 0021 0060 0020 0010 0010 0010 0010 0010 0020 0010 0020 0030 0020 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0020 0010 0010 0010 0010 0010 0010 0020 0020 0010 0010 0010 0010 0020 0020 0020 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0020 0010 0010 0020 0010 0010 0010 09AC");
            txtOut.Close();
        }

        private static void CreateXboxOne()
        {
            if (File.Exists(@"PowerProfiles\xboxOne.txt")) return;
            var txtOut = new StreamWriter(@"PowerProfiles\xboxOne.txt");
            txtOut.WriteLine("You need to create an EventGhost IR Event. Enter this code");
            txtOut.WriteLine("0000 006D 0022 0002 0157 00AC 0015 0016 0015 0016 0015 0016 0015 0016 0015 0016 0015 0016 0015 0016 0015 0041 0015 0016 0015 0016 0015 0016 0015 0041 0015 0041 0015 0016 0015 0041 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0041 0015 0689 0157 0056 0015 0E94");
            txtOut.Close();
        }

        public static void FireEvent(string command)
        {
            var eventGhost = Type.GetTypeFromProgID("EventGhost");
            if (eventGhost == null)
            {
                Debug.Log("EventGhost not found");
                return;
            }

            Debug.Log($"Firing event {command}");
            var myObject = Activator.CreateInstance(eventGhost);
            var Params = new object[] { command, "C# Payload" };
            eventGhost.InvokeMember("TriggerEvent", BindingFlags.InvokeMethod, null, myObject, Params);
        }

    }
}
