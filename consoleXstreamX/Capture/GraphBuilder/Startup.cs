using consoleXstreamX.Capture.Analyse;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    internal static class Startup
    {
        public static void RunGraph()
        {
            VideoCapture.ActiveVideo = false;
            Debug.Log("[0] Build capture graph");

            if (VideoCapture.CurrentVideoDevice <= -1 || (VideoCapture.CurrentVideoDevice > VideoCapture.CaptureDevices.Count))
            {
                Debug.Log($"[0] Unknown capture device {VideoCapture.CurrentVideoDevice}");
                return;
            }

            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];

            VideoCapture.BuildingGraph = true;
            Debug.Log($"Using : {device.Title}");
            Debug.Log("");

            if (VideoCapture.MediaControl != null) VideoCapture.MediaControl.StopWhenReady();
            if (device.Resolution.Count == 0) new CaptureResolution().List();

            VideoCapture.ClearGraph();
            VideoCapture.CaptureGraph = new FilterGraphNoThread() as IGraphBuilder;
            //VideoCapture.CaptureGraph = new FilterGraph() as IGraphBuilder;

            if (!new Graph().Create())
            {
                Debug.Log("[ERR] Error building graph");
                return;
            }

            new Display().Setup();

            VideoCapture.MediaControl = VideoCapture.CaptureGraph as IMediaControl;
            VideoCapture.MediaEvent = VideoCapture.CaptureGraph as IMediaEvent;

            Debug.Log("");
            Debug.Log("Run compiled graph");

            if (VideoCapture.MediaControl != null)
            {
                var hr = VideoCapture.MediaControl.Run();
                Debug.Log("[2] " + DsError.GetErrorText(hr));
            }

            VideoCapture.ActiveVideo = true;
            VideoCapture.BuildingGraph = false;
            Configuration.Settings.CaptureDevice = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice].Title;
            Configuration.Settings.SaveConfiguration();

            if (!VideoCapture.SetRestartGraph) return;
            VideoCapture.SetRestartGraph = false;
            VideoCapture.RestartGraph = true;
            VideoCapture.RestartGraphDelay = 3;
        }
    }
}
