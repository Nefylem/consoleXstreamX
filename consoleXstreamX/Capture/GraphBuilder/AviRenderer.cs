using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class AviRenderer
    {
        public void Create(out string videoIn, out string videoOut, ref string pDevice, ref string pVideoOut, ref IBaseFilter pRen)
        {
            var pin = new Pin();
            Debug.Log("");
            Debug.Log("Creating AVI renderer");
            var pAviDecompressor = (IBaseFilter)new AVIDec();
            var hr = VideoCapture.CaptureGraph.AddFilter(pAviDecompressor, "AVI Decompressor");
            Debug.Log("-> " + DsError.GetErrorText(hr));

            var pinList = pin.List(pAviDecompressor);
            videoIn = pin.Assume("XForm", pinList.In);
            videoOut = pin.Assume("XForm", pinList.Out);

            Debug.Log("");
            Debug.Log($"***   Connect {pDevice} ({pVideoOut}) to AVI Decompressor ({videoIn})");
            hr = VideoCapture.CaptureGraph.ConnectDirect(pin.Get(pRen, pVideoOut), pin.Get(pAviDecompressor, videoIn), null);
            if (hr == 0)
            {
                Debug.Log($"[OK] Connected {pDevice} to AVI Decompressor");
                pRen = pAviDecompressor;
                pDevice = "AVI Decompressor";
                pVideoOut = videoOut;
            }
            else
            {
                Debug.Log($"[FAIL] Can't connected {pDevice} to AVI Decompressor. May interrupt operation");
            }
        }

    }
}
