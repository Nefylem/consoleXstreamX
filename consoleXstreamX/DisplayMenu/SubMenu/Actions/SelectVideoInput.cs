using System;
using System.Linq;
using System.Net.Configuration;
using consoleXstreamX.Capture;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectVideoInput
    {
        public static void Show()
        {
            MenuActions.SetMenu("Video Input");
            MenuActions.ClearSubMenu();

            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            if (device.Crossbars.Count > 0)
            {
                foreach (var item in device.Crossbars)
                {
                    var title = item;
                    if (string.Equals(title, "Video_SerialDigital", StringComparison.CurrentCultureIgnoreCase)) title = "HDMI";
                    if (string.Equals(title, "video_yryby", StringComparison.CurrentCultureIgnoreCase)) title = "Component";
                    if (string.Equals(title, "audio_spdifdigital", StringComparison.CurrentCultureIgnoreCase)) title = "Digital Audio";
                    if (string.Equals(title, "Audio_Line", StringComparison.CurrentCultureIgnoreCase)) title = "Line Audio";
                    Shutter.AddItem(title, title);
                }

                if (Shutter.Tiles.Count > 0)
                    Shutter.Selected = Shutter.Tiles[0].Command;
                else
                {
                    Shutter.Error = "No connections found";
                    Shutter.Explanation = "Your capture device has no avaible connection information";
                }
            }
            else
            {
                Shutter.Error = "No connections found";
                Shutter.Explanation = "Your capture device has no avaible connection information";
            }

            Shutter.SetActiveRow(1);
        }
    }
}
