using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleXstreamX.PowerOn
{
    internal static class PowerStartup
    {
        public static void Create()
        {
            if (!Directory.Exists("PowerProfiles")) Directory.CreateDirectory("PowerProfiles");
            CreatePs3();
            CreateXbox360();
            CreateXboxOne();
        }

        private static void CreatePs3()
        {
            if (File.Exists(@"PowerProfiles\ps3.txt")) return;
            var txtOut = new StreamWriter(@"PowerProfiles\ps3.txt");
            txtOut.WriteLine("0000 006C 0000 001A 0099 00AD 000F 0027 0010 0027 000F 0013 000F 0027 0010 0013 000F 0013 000F 0027 0010 0013 000F 0013 000F 0027 0010 0027 000F 0027 000F 0027 0010 0027 000F 0027 000F 0013 0010 0027 000F 0013 000F 0013 0010 0013 000F 0013 000F 0013 0010 0013 000F 0027 000F 077F");
            txtOut.Close();
        }

        private static void CreateXbox360()
        {
            if (File.Exists(@"PowerProfiles\xbox360.txt")) return;
            var txtOut = new StreamWriter(@"PowerProfiles\xbox360.txt");
            txtOut.WriteLine("0000 0073 0000 0021 0060 0020 0010 0010 0010 0010 0010 0020 0010 0020 0030 0020 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0020 0010 0010 0010 0010 0010 0010 0020 0020 0010 0010 0010 0010 0020 0020 0020 0010 0010 0010 0010 0010 0010 0010 0010 0010 0010 0020 0010 0010 0020 0010 0010 0010 09AC");
            txtOut.Close();
        }

        private static void CreateXboxOne()
        {
            if (File.Exists(@"PowerProfiles\xboxOne.txt")) return;
            var txtOut = new StreamWriter(@"PowerProfiles\xboxOne.txt");
            txtOut.WriteLine("0000 006D 0022 0002 0157 00AC 0015 0016 0015 0016 0015 0016 0015 0016 0015 0016 0015 0016 0015 0016 0015 0041 0015 0016 0015 0016 0015 0016 0015 0041 0015 0041 0015 0016 0015 0041 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0016 0015 0041 0015 0041 0015 0689 0157 0056 0015 0E94");
            txtOut.Close();
        }

    }
}
