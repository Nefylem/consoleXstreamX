using System.Linq;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    internal static class ResolutionController
    {
        private static bool _busy;
        public static int Check()
        {
            if (_busy) return 0;
            if (!VideoCapture.ActiveVideo) return 0;
            if (VideoCapture.BuildingGraph) return 0;
            if (VideoCapture.IamAvd == null) return 0;

            int lineCount;
            VideoCapture.IamAvd.get_NumberOfLines(out lineCount);
            if (lineCount == 0) return 0;

            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            var resolution = device.Resolution[device.CurrentResolution];
            if (lineCount == resolution.Height) return lineCount;
            if (!Configuration.Settings.AutoSetCaptureResolution) return 0;
            _busy = true;
            Change();

            return lineCount;
        }

        public static void Change()
        {
            if (VideoCapture.MediaControl == null) return;
            VideoCapture.SetRestartGraph = true;
            VideoCapture.RunGraph();

            /*
            VideoCapture.MediaControl.Stop();
            */
            //VideoCapture.CaptureDevice = pCaptureDevice;
            //VideoCapture.CaptureFeedOut = pins.Video.Out;
            /*
            VideoCapture.MediaControl.Stop();
            var resolution = new Resolution();
            resolution.Set(VideoCapture.CaptureDevice, VideoCapture.CaptureFeedOut);
            var hr = VideoCapture.MediaControl.Run();
            if (hr < 0)
            {
                var error = DsError.GetErrorText(hr);
                var b = 1;
            }
            */
            _busy = false;
        }

    }
}
