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
        public static void ChangeResolution(int lineCount)
        {
            if (_busy) return;
            _busy = true;
            var b = 1;
        }
    }
}
