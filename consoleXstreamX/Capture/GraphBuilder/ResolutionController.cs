using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace consoleXstreamX.Capture.GraphBuilder
{
    internal static class ResolutionController
    {
        private static bool _busy;
        private static int _numberOfLines;
        public static void Check()
        {
            if (_busy) return;
            if (VideoCapture.IamAvd == null) return;
            int lineCount;
            VideoCapture.IamAvd.get_NumberOfLines(out lineCount);
            if (_numberOfLines == 0) _numberOfLines = lineCount;
            if (_numberOfLines == lineCount) return;
            var b = 1;

            /*

            _numberOfLines = lineCount;
            ResolutionController.ChangeResolution(lineCount);
            */
        }

        public static void ChangeResolution(int lineCount)
        {
            if (_busy) return;
            _busy = true;
        }
    }
}
