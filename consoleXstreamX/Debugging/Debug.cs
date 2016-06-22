using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace consoleXstreamX.Debugging
{
    internal static class Debug
    {
        internal static void Log(string write)
        {
            var path = Configuration.Settings.LogPath;
            if (!string.IsNullOrEmpty(path)) path += @"\";
            path += @"Logs\";

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var sender = new StackFrame(1).GetMethod();
            if (sender?.DeclaringType == null)
            {
                Write("cxsx.log", "Unknown", "Unknown", write);
                return;
            }

            var type = sender.DeclaringType.FullName;
            var name = sender.Name;
            var logFile = SetLogFile(type);
            if (write.IndexOf("[ERR]", StringComparison.CurrentCultureIgnoreCase) > -1) logFile = "error.log";
            Write(path + logFile, type, name, write);
        }

        private static async void Write(string logFile, string type, string name, string write)
        {
            var level = FindLevel(ref write);
            if (level >= 2 || Configuration.Settings.DetailedLogs == 1)
            {
                write = type + "." + name + ": " + write;
            }

            LastDebugLevel = level;
            if (level > Configuration.Settings.SystemDebugLevel) return;

            await WriteTextAsync(logFile, write);
        }

        internal static async Task WriteTextAsync(string file, string write)
        {
            write = write.Trim();
            var strCurrentTime = DateTime.Now.ToString("HH:mm:ss.fff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            var txtOut = new StreamWriter(file, true);
            if (write.Length > 0) write = strCurrentTime + " - " + write;
            await txtOut.WriteLineAsync(write);
            txtOut.Close();
        }

        private static int FindLevel(ref string write)
        {
            if (write.IndexOf('[') <= -1 || write.IndexOf(']') <= -1) return LastDebugLevel;

            var value = write.Substring(1);
            value = value.Substring(0, value.IndexOf(']')).Trim();
            if (!IsNumber(value)) return LastDebugLevel;

            write = write.Substring(write.IndexOf(']') + 1).Trim();
            LastDebugLevel = int.Parse(value);
            return LastDebugLevel;
        }

        private static string SetLogFile(string type)
        {
            if (type.IndexOf(".capture.", StringComparison.CurrentCultureIgnoreCase) > -1) return "video.log";
            return "cxsx.log";
        }

        private static bool IsNumber(string write)
        {
            return write.All(Char.IsDigit);
        }

        private static int LastDebugLevel;

    }
}
