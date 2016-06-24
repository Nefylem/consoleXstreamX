using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Define;

namespace consoleXstreamX.Input.Keyboard
{
    internal static class KeyboardInterface
    {
        private static int _xboxButtonCount = 0;

        public static void Read()
        {
            if (_xboxButtonCount == 0) _xboxButtonCount = Enum.GetNames(typeof(Xbox)).Length;
            var output = new byte[_xboxButtonCount];

        }
    }
}
