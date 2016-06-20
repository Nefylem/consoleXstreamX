using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleXstreamX.Debugging
{
    public class Logging
    {
        public void Cleanup()
        {
            var path = Configuration.Value.LogPath;
            if (!string.IsNullOrEmpty(path)) path += @"\";
            path += @"Logs\";

            var files = Directory.GetFiles(path, "*.log");
            foreach (var item in files)
            {
                File.Delete(item);
            }
        }
    }
}
