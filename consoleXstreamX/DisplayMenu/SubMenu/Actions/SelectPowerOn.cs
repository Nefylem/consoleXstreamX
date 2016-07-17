using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using consoleXstreamX.DisplayMenu.MainMenu;
using consoleXstreamX.PowerOn;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectPowerOn
    {
        private static List<PowerProfiles> Profiles;

        public static void Show()
        {
            MenuActions.SetMenu("Power On");
            MenuActions.ClearSubMenu();

            LoadProfiles();
            if (Shutter.Tiles.Count == 0)
            {
                Shutter.Error = "No power profiles found";
                Shutter.Explanation = "Please create a profile first";
            }
            else

                Shutter.Selected = Shutter.Tiles[0].Command;

            Shutter.SetActiveRow(1);
        }

        private static void LoadProfiles()
        {
            Profiles = new List<PowerProfiles>();

            var value = "";
            var display = "";
            var action = "";
            var command = "";
            var reader = new XmlTextReader(@"PowerProfiles\powerOn.xml");
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
                        var name = reader.Name;
                        if (string.Equals(name, "EventGhost", StringComparison.CurrentCultureIgnoreCase)) { action = name; command = value; }
                        if (string.Equals(name, "Display", StringComparison.CurrentCultureIgnoreCase)) display = value;
                        if (string.Equals(name, "Console", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (Profiles.FirstOrDefault(s => s.Display == display) != null) break;
                            Profiles.Add(new PowerProfiles()
                            {
                                Name = command,
                                Execution = action,
                                Display = display
                            });
                            Shutter.AddItem(display, display);
                        }
                        break;
                }
            }
            reader.Close();
        }

        public static void Run(string command)
        {
            var record = Profiles.FirstOrDefault(s => s.Display == command);
            if (record == null) return;
            if (string.Equals(record.Execution, "EventGhost", StringComparison.CurrentCultureIgnoreCase)) PowerStartup.FireEvent(record.Name);
        }

        private class PowerProfiles
        {
            public string Name;
            public string Execution;
            public string Display;
        }

    }
}
