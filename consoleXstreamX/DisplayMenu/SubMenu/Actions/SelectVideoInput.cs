using System;
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
                var videoPin = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].VideoInput;
                var audioPin = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].AudioInput;
                if (!string.IsNullOrEmpty(videoPin)) Shutter.CheckedItems.Add(videoPin);
                if (!string.IsNullOrEmpty(audioPin)) Shutter.CheckedItems.Add(audioPin);

                foreach (var item in device.Crossbars.Video)
                {
                    var title = item;
                    if (string.Equals(title, "Video_SerialDigital", StringComparison.CurrentCultureIgnoreCase)) title = "HDMI";
                    if (string.Equals(title, "Video_YrYbY", StringComparison.CurrentCultureIgnoreCase)) title = "Component";
                    Shutter.AddItem(title, item, "Video");
                }

                foreach (var item in device.Crossbars.Audio)
                {
                    var title = item;
                    if (string.Equals(title, "audio_spdifdigital", StringComparison.CurrentCultureIgnoreCase)) title = "Digital";
                    if (string.Equals(title, "Audio_Line", StringComparison.CurrentCultureIgnoreCase)) title = "Line";
                    Shutter.AddItem(title, item, "Audio");
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

        public static void Execute(string command)
        {
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            MenuCommand.OkWait = 10;
            if (command.IndexOf("Video_", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                device.CrossbarVideo = command;
                if (string.Equals(command, "Video_SerialDigital", StringComparison.CurrentCultureIgnoreCase)) device.CrossbarAudio = "Audio_SpdifDigital";
                if (string.Equals(command, "Video_YrYbY", StringComparison.CurrentCultureIgnoreCase)) device.CrossbarAudio = "Audio_Line";
            }
            else
                device.CrossbarAudio = command;
            
            Configuration.Settings.CrossbarVideo = device.CrossbarVideo;
            Configuration.Settings.CrossbarAudio = device.CrossbarAudio;
            Configuration.Settings.SaveConfiguration();

            VideoCapture.ChangeCrossbarConnection();
            VideoCapture.MediaControl.Run();

            ResetSelected();
        }

        private static void ResetSelected()
        {
            Shutter.CheckedItems.Clear();
            var videoPin = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].VideoInput;
            var audioPin = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].AudioInput;
            if (!string.IsNullOrEmpty(videoPin)) Shutter.CheckedItems.Add(videoPin);
            if (!string.IsNullOrEmpty(audioPin)) Shutter.CheckedItems.Add(audioPin);
        }
    }
}
