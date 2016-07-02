using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using consoleXstreamX.DisplayMenu;

namespace consoleXstreamX.Resolution
{
    internal static class DisplayResolution
    {
        public static void Change(int height)
        {
            if (Screen.PrimaryScreen.Bounds.Height == height) return;
            var currentResolution = VideoResolution.Get(Configuration.Settings.GraphicsCardId);
            //if (currentResolution.Height == height) return;

            var availableResolution = VideoResolution.List(Configuration.Settings.GraphicsCardId);
            if (availableResolution.Count == 0) return;

            var newResolution = availableResolution.FirstOrDefault(s => s.Height == height);
            if (newResolution == null) return;
            if (newResolution == currentResolution) return;     //Already on target resolution
            VideoResolution.Set(newResolution, Configuration.Settings.GraphicsCardId);
        }

        public static List<string> Get()
        {
            /*
            if (string.IsNullOrEmpty(_class.System.DisplayResolution))
                _class.System.DisplayResolution = _class.VideoResolution.GetDisplayResolution(_class.System.GraphicsCardId);

            return _class.System.DisplayResolution;
            */
            return null;
        } 
    }
}
