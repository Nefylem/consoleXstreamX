using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class VideoRender
    {
        private readonly Guid _clsidActiveVideo = new Guid("{B87BEB7B-8D29-423F-AE4D-6582C10175AC}");

        public void Create(string pVideoOut, IBaseFilter pRen)
        {
            var pin = new Pin();
            Debug.Log("");
            Debug.Log("[0]***   Create Video Renderer");

            var pVideoRenderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(_clsidActiveVideo));
            var hr = VideoCapture.CaptureGraph.AddFilter(pVideoRenderer, "Video Renderer");
            if (hr == 0)
            {
                Debug.Log("[1] [OK] Created video renderer");
            }
            else
            {
                Debug.Log("[1] [FAIL] Cant create video renderer");
                Debug.Log("-> " + DsError.GetErrorText(hr));
            }

            Debug.Log("");
            Debug.Log("***   Listing Video Renderer pins");

            var pinList = pin.List(pVideoRenderer);
            var videoIn = pin.Assume("Input", pinList.In);

            Debug.Log($"<Video> {videoIn}");
            Debug.Log("");

            Debug.Log($"***   Connect AVI Decompressor ({pVideoOut}) to Video Renderer ({videoIn})");
            hr = VideoCapture.CaptureGraph.ConnectDirect(pin.Get(pRen, pVideoOut), pin.Get(pVideoRenderer, videoIn), null);
            if (hr == 0)
            {
                Debug.Log("[OK] Connected AVI to video renderer");
            }
            else
            {
                Debug.Log("[FAIL] Can't connect AVI to video renderer");
                Debug.Log("-> " + DsError.GetErrorText(hr));
            }

            VideoCapture.VideoDef = pVideoRenderer as IBasicVideo;
            VideoCapture.VideoWindow = pVideoRenderer as IVideoWindow;
        }
    }
}
