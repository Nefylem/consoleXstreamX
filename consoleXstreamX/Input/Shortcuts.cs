using System;
using System.Collections.Generic;
using System.ComponentModel;
using consoleXstreamX.Define;
using System.IO;
using consoleXstreamX.Debugging;

namespace consoleXstreamX.Input
{
    public static class Shortcuts
    {
        public class ShortcutItems
        {
            public List<int> OriginKeys; 
            public int TargetKey;
        }

        private static List<ShortcutItems> _shortcuts;

        public static void Load()
        {
            if (_shortcuts == null) _shortcuts = LoadDefaults();
            if (!File.Exists(@"Data\Shortcuts.txt")) CreateShortcutFile();
            var txtIn = new StreamReader(@"Data\Shortcuts.txt");
            string input;
            while ((input = txtIn.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(input)) continue;
                ReadLine(input);
            }
            txtIn.Close();
        }

        private static void ReadLine(string input)
        {
            var originalData = input;
            try
            {
                if (input.IndexOf('=') == -1) return;
                var target = input.Substring(input.LastIndexOf('=') + 1).Trim();
                input = input.Substring(0, input.LastIndexOf('='));
                if (input.IndexOf('}') > -1)
                {
                    input = input.Replace("{", "");
                    input = input.Replace("}", "");
                }
                var origin = input.Split(',');

                var originalValues = new List<int>();
                foreach (var item in origin)
                {
                    var obj = GetValueFromDescription<Xbox>(item.Trim());
                    if (obj == null)
                    {
                        Debug.Log($"[ERR] (shortcuts.txt) Error parsing line '{originalData}' ({item.Trim()} not found)");
                        return;       
                    }
                    originalValues.Add((int)obj);

                }
                var targetObj = GetValueFromDescription<Xbox>(target);
                if (targetObj == null)
                {
                    Debug.Log($"[ERR] (shortcuts.txt) Error parsing line '{originalData}' ({target.Trim()} not found)");
                    return;       
                }

                var targetValue = (int) targetObj;
                var shortcut = new ShortcutItems()
                {
                    OriginKeys = originalValues,
                    TargetKey = targetValue
                };

                if (_shortcuts.IndexOf(shortcut) == -1) _shortcuts.Add(shortcut);
            }
            catch (Exception ex)
            {
                Debug.Log($"[ERR] (shortcuts.txt) Error parsing line '{originalData}' ({ex.Message})");
            }
        }

        public static object GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (string.Equals(attribute.Description, description, StringComparison.CurrentCultureIgnoreCase))
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (string.Equals(field.Name, description, StringComparison.CurrentCultureIgnoreCase))
                        return (T)field.GetValue(null);
                }
            }
            return null;
        }

        private static void CreateShortcutFile()
        {
            if (!Directory.Exists("Data")) Directory.CreateDirectory("Data");
            var txtOut = new StreamWriter(@"Data\shortcuts.txt");
            foreach (var item in _shortcuts)
            {
                var setting = "{";
                foreach (var origin in item.OriginKeys)
                {
                    if (setting.Length > 1) setting += ", ";
                    setting += FindGamepadValue(origin);
                }
                setting += "} = " + FindGamepadValue(item.TargetKey);
                txtOut.WriteLine(setting);
            }
            txtOut.Close();
        }

        private static string FindGamepadValue(int value)
        {
            var xboxValue = (Xbox)value;
            return GetEnumDescription(xboxValue);
        }

        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi == null) return value.ToString();
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        private static List<ShortcutItems> LoadDefaults()
        {
            return new List<ShortcutItems>
            {
                new ShortcutItems()
                {
                    OriginKeys = new List<int>()
                    {
                        (int) Xbox.Back,
                        (int) Xbox.B,
                    },
                    TargetKey = (int) Xbox.Home
                },

                new ShortcutItems()
                {
                    OriginKeys = new List<int>()
                    {
                        (int) Xbox.Touch,
                        (int) Xbox.B,
                    },
                    TargetKey = (int) Xbox.Home
                }
            };
        }

        public static void Check(ref byte[] output)
        {
            if (_shortcuts == null) return;

            foreach (var item in _shortcuts)
            {
                var found = true;
                foreach (var origin in item.OriginKeys)
                {
                    if (output[origin] != 100) found = false;
                }
                if (!found) continue;

                //Clear original button keys
                foreach (var origin in item.OriginKeys)
                {
                    output[origin] = Convert.ToByte(0);
                }
                //Set target key
                output[item.TargetKey] = Convert.ToByte(100);

            }
        }
    }
}
