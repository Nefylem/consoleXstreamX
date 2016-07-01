using System;

namespace consoleXstreamX.Capture.Settings
{
    class UserSettings
    {
        public void Read()
        {
            //Defaults
            VideoCapture.UseCrossbar = true;
            VideoCapture.CreateSmartTee = false;
            VideoCapture.CreateAviRenderer = true;
            VideoCapture.UseSampleGrabber = false;
            VideoCapture.CurrentAudioDevice = 0;
            VideoCapture.CurrentVideoDevice = 0;

            //Load user settings
            SetVideo();
            SetAudio();
            SetCrossbar();
        }

        public void SetVideo()
        {
            if (string.IsNullOrEmpty(Configuration.Settings.CaptureDevice)) return;
            var index = VideoCapture.CaptureDevices.FindIndex(s => string.Equals(s.Title, Configuration.Settings.CaptureDevice, StringComparison.CurrentCultureIgnoreCase));
            if (index > -1) VideoCapture.CurrentVideoDevice = index;
        }

        public void SetAudio()
        {
            if (string.IsNullOrEmpty(Configuration.Settings.CaptureAudio)) Configuration.Settings.CaptureAudio = "Default WaveOut Device";

            if (VideoCapture.AudioDevices == null || VideoCapture.AudioDevices.Count == 0) return;
            var index = VideoCapture.AudioDevices.IndexOf(Configuration.Settings.CaptureAudio);
            if (index == -1)
            {
                VideoCapture.CurrentAudioDevice = 0;
                return;
            }
            VideoCapture.CurrentAudioDevice = index;
        }

        public void SetCrossbar()
        {
            //No need to iterate through these. The graph will do it. just pass the information along
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            device.CrossbarVideo = Configuration.Settings.CrossbarVideo;
            device.CrossbarAudio = Configuration.Settings.CrossbarAudio;
        }
    }
}
