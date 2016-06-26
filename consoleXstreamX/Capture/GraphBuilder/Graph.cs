using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Graph
    {
        public bool Create()
        {
            var previewIn = "";
            var previewOut = "";

            var crossbar = new Crossbar();

            if (VideoCapture.CurrentVideoDevice <= -1 || (VideoCapture.CurrentVideoDevice > VideoCapture.CaptureDevices.Count))
            {
                Debug.Log($"[0] Unknown capture device {VideoCapture.CurrentVideoDevice}");
                return false;
            }

            new PrimaryDevice().Create();
            new GraphBuilder().Create();
            var pCaptureDevice = new Capture().Create();
            var pins = new VideoConnectors().Make(pCaptureDevice);

            VideoCapture.IamAvd = pCaptureDevice as IAMAnalogVideoDecoder;

            if (VideoCapture.UseCrossbar)
            {
                var xbar = crossbar.Create(pins.Video.In, pins.Audio.In, VideoCapture.CurrentVideoShort, pCaptureDevice);
                if (xbar.Found) Crossbar.Check();
            }

            new VideoResolution().Set(pCaptureDevice, pins.Video.Out);
            var pRen = pCaptureDevice;
            var pDevice = VideoCapture.CurrentVideo;
            var pVideoOut = pins.Video.Out;

            if (VideoCapture.UseSampleGrabber) new SampleGrabber().Create(ref previewIn, ref previewOut, ref pDevice, ref pVideoOut, ref pRen);
            if (VideoCapture.CreateSmartTee)
            {
                new SmartTee().Create(out previewIn, out previewOut, ref pDevice, ref pVideoOut, ref pRen);

                VideoCapture.CaptureFeed = pRen;
                VideoCapture.CaptureFeedIn = previewIn;
            }

            if (VideoCapture.CreateAviRenderer) new AviRenderer().Create(pDevice, ref pVideoOut, ref pRen);
            new VideoRender().Create(pVideoOut, pRen);
            new AudioRender().Create(pins.Audio.Out, pCaptureDevice);

            return true;
        }


    }
}
