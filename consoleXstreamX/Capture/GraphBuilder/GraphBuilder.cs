using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class GraphBuilder
    {
        public void Create()
        {
            Debug.Log("");
            Debug.Log("[0] Create new graph");
            var pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            var hr = pBuilder.SetFiltergraph(VideoCapture.CaptureGraph);
            Debug.Log("[2] [OK] " + DsError.GetErrorText(hr));
            Debug.Log("");
        }

    }
}
