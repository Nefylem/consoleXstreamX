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
            _busy = true;
            Change();
            /*
            var targetResolution = device.Resolution.FirstOrDefault(s => s.Height == lineCount);
            if (targetResolution == null) return;
            _busy = true;
            Change(targetResolution);
            _busy = false;
            */
            return lineCount;
        }

        public static void Change()
        {
            if (VideoCapture.MediaControl == null) return;
            VideoCapture.MediaControl.Stop();
            VideoCapture.ClearGraph();
            VideoCapture.RunGraph();
            _busy = false;
            /*
            VideoCapture.MediaControl.Stop();
            var pin = new Pin();
            var hr = ((IAMStreamConfig)pin.Get(VideoCapture.CaptureDevice, VideoCapture.CaptureFeedOut)).SetFormat(targetResolution.MediaType);
            if (hr != 0)
            {
                Debug.Log($"[NG] Failed to set resolution {targetResolution.GetRes()}");
                Debug.Log("-> " + DsError.GetErrorText(hr));
                return;
            }
            Debug.Log($"[OK] Changed resolution to {targetResolution.GetRes()}");
            VideoCapture.MediaControl.Run();
            */
        }

    }
}
