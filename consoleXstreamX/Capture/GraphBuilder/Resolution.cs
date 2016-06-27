using System.Linq;
using consoleXstreamX.Debugging;
using consoleXstreamX.Resolution;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Resolution
    {
        public int Get()
        {
            if (VideoCapture.IamAvd == null) return 0;
            int lineCount;
            VideoCapture.IamAvd.get_NumberOfLines(out lineCount);

            if (lineCount <= 0) return 0;

            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            var res = device.Resolution.FirstOrDefault(s => s.Height == lineCount);
            if (res != null) VideoCapture.CurrentResolutionIndex = res.Index;
            //DisplayResolution.Change(lineCount);

            //todo: check if the avi renderer is breaking the graph at low resolutions
            return VideoCapture.CurrentResolutionIndex;
        }

        public int Set(IBaseFilter pCaptureDevice, string captureVideoOut)
        {
            var pin = new Pin();
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            if (VideoCapture.CurrentResolutionIndex > device.Resolution.Count)
            {
                Debug.Log($"[0] [ERR] cant find resolution {VideoCapture.CurrentResolutionIndex}");
                return -99999;
            }

            Debug.Log($"[3] set resolution {device.Resolution[VideoCapture.CurrentResolutionIndex].GetRes()}");
            VideoCapture.Resolution = device.Resolution[VideoCapture.CurrentResolutionIndex].MediaType;
            device.CurrentResolution = VideoCapture.CurrentResolutionIndex;
            var hr = ((IAMStreamConfig)pin.Get(pCaptureDevice, captureVideoOut)).SetFormat(VideoCapture.Resolution);
            if (hr == 0)
            {
                Debug.Log($"[OK] Set resolution {device.Resolution[VideoCapture.CurrentResolutionIndex].GetRes()}");
                return 0;
            }

            Debug.Log($"[NG] Can't set resolution {device.Resolution[VideoCapture.CurrentResolutionIndex].GetRes()}");
            Debug.Log("-> " + DsError.GetErrorText(hr));
            return hr;
        }

    }
}
