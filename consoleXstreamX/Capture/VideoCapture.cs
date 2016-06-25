﻿using System.Collections.Generic;
using consoleXstreamX.Capture.GraphBuilder;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture
{
    internal static class VideoCapture
    {
        internal static void Startup(Form1 form1)
        {
            Home = form1;
            new Startup().Execute();
        }

        public static void ResetDisplay()
        {
            new Display().Setup();
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
        public static IBaseFilter CaptureDevice;
        public static IBaseFilter CaptureFeed;
        public static AMMediaType Resolution;

        public class VideoCaptureDevices
        {
            public string Title;
            public int CurrentResolution;
            public List<VideoCaptureResolution> Resolution;

            public List<string> Crossbars
            {
                get
                {
                    if (_crossbar == null || _crossbar.Count == 0) _crossbar = Crossbar.ListOutputs();
                    return _crossbar;
                }
            }

            private List<string> _crossbar;
        }

        public class VideoCaptureResolution
        {
            public AMMediaType MediaType;
            public string Type;
            public int Width;
            public int Height;

            public string GetRes()
            {
                return $"{Width} x {Height} ({Type})";
            }
        }
    }
}
