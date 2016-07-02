using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using consoleXstreamX.Capture.Analyse;
using consoleXstreamX.Capture.GraphBuilder;
using consoleXstreamX.Capture.Settings;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture
{
    internal static class VideoCapture
    {
        internal static void Startup(Form1 form1)
        {
            if (form1 != null) Home = form1;

            new VideoInput().Find();
            new AudioRenderer().Find();
            new UserSettings().Read();
            new CaptureResolution().List();

            InitializeGraph = true;
            RestartGraph = true;

            GraphBuilder.Startup.RunGraph();
        }

        public static void RunGraph()
        {
            GraphBuilder.Startup.RunGraph();
        }

        public static void ChangeCrossbarConnection()
        {
            Crossbar.Check();
        }

        public static int CheckResolution()
        {
            return ResolutionController.Check();
        }

        public static void SetWait()
        {
            var image = Home.wait;
            var show = new Bitmap(Home.Width, Home.Height);
            var graph = Graphics.FromImage(show);
            var blueBrush = new SolidBrush(Color.Red);
            graph.FillRectangle(blueBrush, 0, 0, show.Width, show.Height);
            //todo: Show wait image
            Home.wait.Image = show;
            Home.wait.Visible = true;
            Home.display.Visible = false;
            Application.DoEvents();
        }

        public static void ResetDisplay()
        {
            new GraphBuilder.Display().Setup();
        }

        public static void CloseGraph()
        {
            Debug.Log("[0] [TRY] Gracefully closing graph");
            MediaControl?.StopWhenReady();
            ClearGraph();
            Debug.Log("[OK] Graph closed");
        }

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

        public static Form1 Home;
        public static int CurrentVideoDevice;
        public static int CurrentAudioDevice;
        public static int CurrentActiveDevice;
        public static int CurrentResolutionIndex;

        public static string CurrentVideo;
        public static string CurrentVideoShort;
        public static string CurrentAudio;
        public static string CurrentResolution;

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
        public static bool ActiveCrossbar;

        public static IMediaControl MediaControl;
        public static IGraphBuilder CaptureGraph;
        public static IMediaEvent MediaEvent;
        public static IAMAnalogVideoDecoder IamAvd;
        public static IBasicVideo VideoDef;
        public static IVideoWindow VideoPreview;
        public static IAMCrossbar XBar;
        public static IVideoWindow VideoWindow;

        public static string CaptureFeedIn;
        public static string CaptureFeedOut;
        public static IBaseFilter CaptureDevice;
        public static IBaseFilter CaptureFeed;
        public static AMMediaType Resolution;

        public class VideoCaptureDevices
        {
            public string Title;

            public string CrossbarAudio;
            public string CrossbarVideo;
            public int CurrentResolution;
            public List<VideoCaptureResolution> Resolution;

            public string VideoInput
            {
                get
                {
                    var crossbarList = Crossbar.ListOutputByType();
                    var videoPin = Crossbar.GetActivePin("Video");
                    return videoPin < crossbarList.Video.Count ? crossbarList.Video[videoPin] : "";
                }
            }

            public string AudioInput
            {
                get
                {
                    var crossbarList = Crossbar.ListOutputByType();
                    var audioPin = Crossbar.GetActivePin("Audio") - crossbarList.Video.Count;
                    return audioPin < crossbarList.Audio.Count ? crossbarList.Audio[audioPin] : "";
                }
            }

            public Crossbar.CrossbarItems Crossbars
            {
                get
                {
                    if (_crossbar == null) _crossbar = Crossbar.ListOutputByType();
                    return _crossbar;
                }
            }

            private Crossbar.CrossbarItems _crossbar;
        }

        public class VideoCaptureResolution
        {
            public AMMediaType MediaType;
            public string Type;
            public int Width;
            public int Height;
            public int Index;

            public string GetRes()
            {
                return $"{Width} x {Height} ({Type})";
            }
        }
    }
}
