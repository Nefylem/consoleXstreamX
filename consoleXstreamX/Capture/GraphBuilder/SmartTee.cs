using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class SmartTee
    {
        public void Create(out string previewIn, out string previewOut, ref string pDevice, ref string pVideoOut, ref IBaseFilter pRen)
        {
            var pin = new Pin();
            Debug.Log("");
            Debug.Log("Creating SmartTee Preview Filter");

            var pSmartTee2 = (IBaseFilter)new DirectShowLib.SmartTee();
            var hr = VideoCapture.CaptureGraph.AddFilter(pSmartTee2, "Smart Tee");
            Debug.Log(DsError.GetErrorText(hr));
            Debug.Log("");

            var pinList = pin.List(pSmartTee2);
            previewIn = pin.Assume("Input", pinList.In);
            previewOut = pin.Assume("Preview", pinList.Out);

            Debug.Log("");
            Debug.Log("***   Connect " + pDevice + " (" + pVideoOut + ") to SmartTee Preview Filter (" + previewIn + ")");
            hr = VideoCapture.CaptureGraph.ConnectDirect(pin.Get(pRen, pVideoOut), pin.Get(pSmartTee2, previewIn), null);
            if (hr == 0)
            {
                Debug.Log("[OK] Connected " + pDevice + " to SmartTee Preview Filter");
                pDevice = "SmartTee Preview Filter";
                pRen = pSmartTee2;
                pVideoOut = previewOut;
            }
            else
            {
                Debug.Log("[NG] cant Connect " + pDevice + " to Preview Filter. Attempting to continue without preview");
                Debug.Log("-> " + DsError.GetErrorText(hr));
            }
        }
    }
}
