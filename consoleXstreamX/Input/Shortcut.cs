using System.Collections.Generic;

namespace consoleXstreamX.Input
{
    public static class Shortcut
    {
        private static class ShortcutItems
        {
            public static List<int> OriginKeys; 
            public static int TargetKey;
        }
        /*
                private int[,] _intShortcut;
        private int _intShortcutCount;

        private void LoadShortcutKeys()
        {
            _intShortcutCount = 0;

            string strInput;
            _intShortcut = new int[3, 25];

            //Load these in incase there is no shortcut file
            //Home - normal mode
            _intShortcut[0, 0] = (int)Xbox.Back;
            _intShortcut[1, 0] = (int)Xbox.B;
            _intShortcut[2, 0] = (int)Xbox.Home;
            _intShortcutCount++;

            //Home - PS4 mode
            _intShortcut[0, 1] = (int)Xbox.Touch;
            _intShortcut[1, 1] = (int)Xbox.B;
            _intShortcut[2, 1] = (int)Xbox.Home;
            _intShortcutCount++;

            if (File.Exists(@"Data\shortcutGamepad.txt") != true) return;
            var intId = 0;
            var txtIn = new StreamReader(@"Data\shortcutGamepad.txt");
            while ((strInput = txtIn.ReadLine()) != null)
            {
                try
                {
                    var strTemp = strInput.Split(',');
                    if (strTemp.Length != 3) continue;
                    var intTemp1 = Convert.ToInt32(strTemp[0]);
                    var intTemp2 = Convert.ToInt32(strTemp[1]);
                    var intTemp3 = Convert.ToInt32(strTemp[2]);

                    _intShortcut[0, intId] = intTemp1;
                    _intShortcut[1, intId] = intTemp2;
                    _intShortcut[2, intId] = intTemp3;

                    intId++;
                    _intShortcutCount++;
                }
                catch
                {
                    // ignored
                }
            }
            txtIn.Close();
        }
        */
        public static void Check(ref byte[] output)
        {
            /*
            for (var intCount = 0; intCount < _intShortcutCount; intCount++)
            {
                var intData1 = _intShortcut[0, intCount];
                var intData2 = _intShortcut[1, intCount];
                var intTarget = _intShortcut[2, intCount];

                if ((output[intData1].ToString() != "100") || (output[intData2].ToString() != "100")) continue;
                output[intData1] = Convert.ToByte(0);
                output[intData2] = Convert.ToByte(0);

                if (intTarget < 32)
                    output[intTarget] = Convert.ToByte(100);
            }

            return output;
            */
        }
    }
}
