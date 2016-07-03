using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class VideoResolution
    {
        public void Set(IBaseFilter pCaptureDevice, string videoOut)
        {
            Debug.Log("[0] Checking capture resolution");
            var index = Resolution.Get();

            if (index > 0)
            {
                Resolution.Set(pCaptureDevice, videoOut);
                return;
            }
            Debug.Log("[0] [WARN] Cant find capture resolution - no input or unknown resolution type");
        }

    }
}
