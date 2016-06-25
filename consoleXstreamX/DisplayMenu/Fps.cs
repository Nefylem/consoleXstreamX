using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Drawing;

namespace consoleXstreamX.DisplayMenu
{
    //Used to regulate control timing. Otherwise cursor moves to fast to select item
    internal static class Fps
    {
        public static int Count;

        private static int _currentFrames;
        private static string _lastTimeCheck;

        public static void Check()
        {
            if (DateTime.Now.ToString("ss") == _lastTimeCheck) { _currentFrames++; return; }
            Count = _currentFrames;
            _currentFrames = 0;
            _lastTimeCheck = DateTime.Now.ToString("ss");
        }

        public static void Show()
        {
            Draw.Outline = false;
            Draw.FontSize = 12f;
            Draw.Text(10, 10, $"FPS: {Fps.Count}");
        }

    }
}
