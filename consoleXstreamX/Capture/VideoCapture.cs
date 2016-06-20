using System.Collections.Generic;
using DirectShowLib;

namespace consoleXstreamX.Capture
{
    internal static class VideoCapture
    {
        internal static void Startup()
        {
            new Startup().Execute();
            var b = 1;
        }

        public static int CurrentVideoDevice;
        public static int CurrentAudioDevice;

        public static string CrossbarAudio;
        public static string CrossbarVideo;

        public static List<VideoCaptureDevices> CaptureDevices;
        public static List<string> AudioDevices;

        public static bool UseCrossbar;
        public static bool CreateSmartTee;
        public static bool CreateAviRenderer;
        public static bool UseSampleGrabber;

        public static bool BuildingGraph;
        public static bool InitializeGraph;
        public static bool RestartGraph;
        public static bool ActiveVideo;

        public static IMediaControl MediaControl;
        public static IGraphBuilder CaptureGraph;
        public static IMediaEvent MediaEvent;
        public static IAMAnalogVideoDecoder IamAvd;
        public static IBasicVideo VideoDef;
        public static IVideoWindow VideoPreview;
        public static IAMCrossbar XBar;
        public static IVideoWindow VideoWindow;

        public static IBaseFilter CaptureDevice;
        public static IBaseFilter CaptureFeed;
        public static string CaptureFeedIn;
        public static AMMediaType Resolution;

        public static void ClearGraph()
        {
            CaptureGraph = null;
            MediaControl = null;
            MediaEvent = null;
            IamAvd = null;
            VideoDef = null;
            VideoPreview = null;
            XBar = null;
            VideoWindow = null;
        }

        public class VideoCaptureDevices
        {
            public string Title;
            public List<VideoCaptureResolution> Resolution;
        }

        public class VideoCaptureResolution
        {
            public AMMediaType MediaType;
            public string Type;
            public int Width;
            public int Height;
        }
    }
}
