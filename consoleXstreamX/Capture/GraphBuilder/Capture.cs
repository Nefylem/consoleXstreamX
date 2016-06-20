using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Capture
    {
        public IBaseFilter Create()
        {
            string temp;

            var pCaptureDevice = new Filter().Set(FilterCategory.VideoInputDevice, VideoCapture.CurrentVideo, out temp);
            if (pCaptureDevice == null)
            {
                Debug.Log("");
                Debug.Log("[ERR] Cant create capture device. Graph cannot continue");
                return null;
            }
            VideoCapture.CaptureDevice = pCaptureDevice;
            return pCaptureDevice;
        }


    }
}
