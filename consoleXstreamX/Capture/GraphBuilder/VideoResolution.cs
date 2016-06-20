using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class VideoResolution
    {
        public void Set(IBaseFilter pCaptureDevice, string videoOut)
        {
            Debug.Log("[0] Checking capture resolution");
            var resolution = new Resolution();
            var index = resolution.Get();

            if (index > 0)
            {
                resolution.Set(pCaptureDevice, videoOut);
                return;
            }
            Debug.Log("[0] [WARN] Cant find capture resolution - no input or unknown resolution type");
        }

    }
}
