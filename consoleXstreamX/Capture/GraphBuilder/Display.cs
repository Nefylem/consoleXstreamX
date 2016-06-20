using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Display
    {
        public void Setup()
        {
            if (VideoCapture.Home == null) return;

            var display = VideoCapture.Home.display;
            var video = VideoCapture.VideoWindow;
            try
            {
                video.put_Owner(display.Handle);
                video.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
                video?.SetWindowPosition(0, 0, display.Width, display.Height);
                video.SetWindowForeground(OABool.True);
                video?.SetWindowPosition(0, 0, display.Width, display.Height);
                video.put_Visible(OABool.True);
                VideoCapture.Home.FocusWindow();
            }
            catch (Exception ex)
            {
                Debug.Log("[0] [ERR] Problem attaching window");
                Debug.Log($"-> {ex.Message}");
            }
        }
    }
}
