using System;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class SampleGrabber
    {
        private readonly Guid _clsidSampleGrabber = new Guid("{C1F400A0-3F08-11D3-9F0B-006008039E37}");

        public void Create(ref string previewIn, ref string previewOut, ref string pDevice, ref string pVideoOut, ref IBaseFilter pRen)
        {
            var pin = new Pin();
            Debug.Log("");
            Debug.Log("Creating SampleGrabber");

            var pSampleGrabber = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(_clsidSampleGrabber));
            var hr = VideoCapture.CaptureGraph.AddFilter(pSampleGrabber, "SampleGrabber");
            if (hr != 0) Debug.Log("-> " + DsError.GetErrorText(hr));

            var pinList = pin.List(pSampleGrabber);
            var sampleIn = pin.AssumePin("Input", pinList.In);
            var sampleOut = pin.AssumePin("Output", pinList.Out);

            Debug.Log("Set samplegrabber resolution feed");
            if (VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].Resolution.Count > 0)
            {
                var res = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].Resolution[VideoCapture.CurrentResolutionIndex];
                hr = ((ISampleGrabber)pSampleGrabber).SetMediaType(res.MediaType);
                Debug.Log("-> " + DsError.GetErrorText(hr));
            }
            else
                Debug.Log("[ERR] failure in video resolution list");

            Debug.Log("");
            Debug.Log("***   Connect " + pDevice + " (" + pVideoOut + ") to SampleGrabber (" + sampleIn + ")");
            hr = VideoCapture.CaptureGraph.ConnectDirect(pin.Get(pRen, "Capture"), pin.Get(pSampleGrabber, "Input"), null);
            if (hr == 0)
            {
                var cb = new SampleGrabberCallback();
                cb.GetForm1Handle(VideoCapture.Home);

                var sampleGrabber = (ISampleGrabber)pSampleGrabber;
                sampleGrabber.SetCallback(cb, 1);

                Debug.Log("[OK] Connected " + pDevice + " to SampleGrabber");
                pDevice = "Sample Grabber";
                pRen = pSampleGrabber;
                pVideoOut = sampleOut;
            }
            else
            {
                Debug.Log("[NG] Cant connect SampleGrabber to video Capture feed. Attempting to continue.");
                Debug.Log("-> " + DsError.GetErrorText(hr));
            }
        }
    }
}
