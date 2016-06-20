using consoleXstreamX.Debugging;
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

            //System.AutoChangeResolution(lineCount);
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            for (var count = 0; count < device.Resolution.Count; count++)
            {
                if (lineCount == device.Resolution[count].Height)
                {
                    VideoCapture.CurrentResolutionIndex = count;
                }
            }
            return VideoCapture.CurrentResolutionIndex;
        }

        public int Set(IBaseFilter pCaptureDevice, string strCaptureVideoOut)
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

            var hr = ((IAMStreamConfig)pin.Get(pCaptureDevice, strCaptureVideoOut)).SetFormat(VideoCapture.Resolution);
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
