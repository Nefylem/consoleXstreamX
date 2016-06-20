using System;
using System.Collections.Generic;
using System.Linq;
using consoleXstreamX.Capture.Settings;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Graph
    {
        public bool BuildGraph()
        {
            var previewIn = "";
            var previewOut = "";
            var videoIn = "";
            var videoOut = "";

            var crossbar = new Crossbar();

            if (VideoCapture.CurrentVideoDevice <= -1 || (VideoCapture.CurrentVideoDevice > VideoCapture.CaptureDevices.Count))
            {
                Debug.Log($"[0] Unknown capture device {VideoCapture.CurrentVideoDevice}");
                return false;
            }

            CreateDevice();
            CreateGraph();
            var pCaptureDevice = CreateVideoCapture();
            var pins = AssignVideoConnectors(pCaptureDevice);

            VideoCapture.IamAvd = pCaptureDevice as IAMAnalogVideoDecoder;

            if (VideoCapture.UseCrossbar)
            {
                var xbar = crossbar.Create(pins.Video.In, pins.Audio.In, VideoCapture.CurrentVideoShort, pCaptureDevice);
                if (xbar.Found) crossbar.Check();
            }

            SetCaptureResolution(pCaptureDevice, pins.Video.Out);

            var pRen = pCaptureDevice;
            var pDevice = VideoCapture.CurrentVideo;
            var pVideoOut = pins.Video.Out;

            if (VideoCapture.UseSampleGrabber) new SampleGrabber().Create(ref previewIn, ref previewOut, ref pDevice, ref pVideoOut, ref pRen);
            if (VideoCapture.CreateSmartTee)
            {
                new SmartTee().Create(out previewIn, out previewOut, ref pDevice, ref pVideoOut, ref pRen);

                VideoCapture.CaptureFeed = pRen;
                VideoCapture.CaptureFeedIn = previewIn;
            }

            if (VideoCapture.CreateAviRenderer) new AviRenderer().Create(out videoIn, out videoOut, ref pDevice, ref pVideoOut, ref pRen);
            new VideoRender().Create(pVideoOut, pRen);
            new AudioRender().Create(pins.Audio.Out, pCaptureDevice);
            return true;
        }

        /*
         * Looks for a unique identifier (eg Avermedia U3 extremecap will return Avermedia)
         * If there's more than one avermedia card, will look for the next available unique name
         * Finally, if there's two avermedia u3 on there, will still only return Avermedia
         */
        private string FindCaptureName(string device)
        {
            var output = "";
            foreach (var t in device)
            {
                if (t == ' ')
                {
                    if (FindCaptureResults(output) == 1)
                        break;
                    else
                        output += t;
                }
                else output += t;
            }

            return output;
        }

        private static int FindCaptureResults(string name)
        {
            var result = 0;
            var listDev = new List<string>();

            foreach (var devName in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).Select(device => device.Name.ToLower()).Where(devName => listDev.IndexOf(devName) == -1))
            {
                listDev.Add(devName);

                if (devName.Length <= name.Length) continue;
                if (devName.Substring(0, name.Length) == name.ToLower())
                    result++;
            }

            listDev.Clear();

            return result;
        }


        private void CreateDevice()
        {
            var device = VideoCapture.CaptureDevices[VideoCapture.CurrentVideoDevice];
            if (device.CurrentResolution > device.Resolution.Count) device.CurrentResolution = 0;

            if (VideoCapture.CurrentAudioDevice > VideoCapture.AudioDevices.Count) new UserSettings().SetAudio();

            VideoCapture.CurrentVideo = device.Title;
            VideoCapture.CurrentVideoShort = FindCaptureName(device.Title);
            VideoCapture.CurrentAudio = VideoCapture.AudioDevices[VideoCapture.CurrentAudioDevice];
            VideoCapture.CurrentResolution = device.Resolution[device.CurrentResolution].GetRes();

            Debug.Log($"[2] Video Capture Device: {VideoCapture.CurrentVideo}");
            Debug.Log($"[2] Video Capture Device ID: {VideoCapture.CurrentVideoShort}");
            Debug.Log($"[2] Resolution: {VideoCapture.CurrentResolution})");
            Debug.Log($"[2] Audio Renderer: {VideoCapture.CurrentAudio}");
        }

        private void CreateGraph()
        {
            Debug.Log("");
            Debug.Log("[0] Create new graph");
            var pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            var hr = pBuilder.SetFiltergraph(VideoCapture.CaptureGraph);
            Debug.Log("[2] [OK] " + DsError.GetErrorText(hr));
            Debug.Log("");
        }

        private IBaseFilter CreateVideoCapture()
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

        private Pin.CapturePins AssignVideoConnectors(IBaseFilter pCaptureDevice)
        {
            var pin = new Pin();
            var result = new Pin.CapturePins
            {
                Video = new Pin.Node(),
                Audio = new Pin.Node()
            };

            var pinList = pin.List(pCaptureDevice);
            result.Video.Out = pin.Assume("Capture", "Video", pinList.Out);
            result.Audio.Out = pin.Assume("Audio", pinList.Out);

            result.Video.In = pin.Assume("Video", pinList.In);
            result.Audio.In = pin.Assume("Audio", pinList.In);

            //Alias for Deen0x's spanish card
            if (string.IsNullOrEmpty(result.Video.Out)) result.Video.Out = pin.Assume("Capturar", "vídeo", pinList.Out);     
            if (string.IsNullOrEmpty(result.Video.In)) result.Video.In = pin.Assume("Capturar", "vídeo", pinList.In);

            Debug.Log("[0]");
            Debug.Log("<Video Out>" + result.Video.Out);
            Debug.Log("<Audio Out>" + result.Audio.Out);
            Debug.Log("<Video In>" + result.Video.In);
            Debug.Log("<Audio In>" + result.Audio.In);
            Debug.Log("");

            return result;
        }

        private void SetCaptureResolution(IBaseFilter pCaptureDevice, string videoOut)
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
