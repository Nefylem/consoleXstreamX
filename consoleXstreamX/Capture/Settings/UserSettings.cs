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
            VideoCapture.UseSampleGrabber = true;
            VideoCapture.CurrentAudioDevice = 0;
            VideoCapture.CurrentVideoDevice = 0;

            //Load user settings
            SetAudio();
        }

        public void SetAudio()
        {
            if (VideoCapture.AudioDevices == null || VideoCapture.AudioDevices.Count == 0) return;
            var index = VideoCapture.AudioDevices.IndexOf("Default WaveOut Device");
            if (index == -1)
            {
                VideoCapture.CurrentAudioDevice = 0;
                return;
            }
            VideoCapture.CurrentAudioDevice = index;
        }
    }
}
