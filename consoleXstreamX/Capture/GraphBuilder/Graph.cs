using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Graph
    {
        public bool BuildGraph()
        {
            /*
            if (_class.Capture.CurrentDevice <= -1 ||
                _class.Capture.CurrentDevice >= _class.Var.VideoCaptureDevice.Count) return true;

            var strVideoDevice = _class.Var.VideoCaptureDevice[_class.Capture.CurrentDevice];
            var strShortName = FindCaptureName(strVideoDevice);

            if (_class.Var.VideoResolutionIndex > _class.Resolution.List.Count)
                _class.Var.VideoResolutionIndex = 0;

            if (_class.Audio.Output > _class.Audio.Devices.Count)
            {
                _class.Audio.Output = -1;
                _class.Audio.Find();
            }

            _class.Debug.Log("[2] VCD: " + strVideoDevice);
            _class.Debug.Log("[2] VCDID: " + strShortName);
            _class.Debug.Log("[2] RES: " + _class.Resolution.List[_class.Var.VideoResolutionIndex]);
            _class.Debug.Log("[2] AOD: " + _class.Audio.Devices[_class.Audio.Output]);

            _class.Var.VideoDevice = strVideoDevice;
            _class.Var.AudioDevice = _class.Audio.Devices[_class.Audio.Output];

            //Filter lists definitions
            var strCrossVideoOut = "";
            var strCrossAudioOut = "";
            var strAvIin = "";
            var strAvIout = "";
            var strVideoIn = "";
            var strPreviewIn = "";
            var strPreviewOut = "";
            var strTempOut = "";

            //graph builder
            _class.Debug.Log("");
            _class.Debug.Log("[0] Create new graph");
            // ReSharper disable once SuspiciousTypeConversion.Global
            var pBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            var hr = pBuilder.SetFiltergraph(_class.Graph.CaptureGraph);
            _class.Debug.Log("[2] [OK] " + DsError.GetErrorText(hr));
            _class.Debug.Log("");

            //_class.Graph.VideoWindow = (IVideoWindow)_class.Graph.CaptureGraph;            //Open the window

            //Primary Capture Device
            var pCaptureDevice = _class.GraphFilter.Set(FilterCategory.VideoInputDevice, strVideoDevice, out strTempOut);
            _class.Debug.Log("");
            if (pCaptureDevice == null)
            {
                _class.Debug.Log("[ERR] Cant create capture device. Graph cannot continue");
                return false;
            }

            _class.Graph.CaptureDevice = pCaptureDevice;

            //Video capture in/output
            _class.GraphPin.ListPin(pCaptureDevice);
            var strCaptureVideoOut = _class.GraphPin.AssumePinOut("Capture", "Video");
            if (strCaptureVideoOut.Length == 0) strCaptureVideoOut = _class.GraphPin.AssumePinOut("Capturar", "vídeo");     //Alias for Deen0x spanish card
            var strCaptureAudioOut = _class.GraphPin.AssumePinOut("Audio");

            var strCaptureVideoIn = _class.GraphPin.AssumePinIn("Video");
            if (strCaptureVideoIn.Length == 0) strCaptureVideoIn = _class.GraphPin.AssumePinIn("Capturar", "vídeo");

            var strCaptureAudioIn = _class.GraphPin.AssumePinIn("Audio");

            _class.Debug.Log("[0]");
            _class.Debug.Log("<Video Out>" + strCaptureVideoOut);
            _class.Debug.Log("<Audio Out>" + strCaptureAudioOut);
            _class.Debug.Log("<Video In>" + strCaptureVideoIn);
            _class.Debug.Log("<Audio In>" + strCaptureAudioIn);
            _class.Debug.Log("");

            // ReSharper disable once SuspiciousTypeConversion.Global
            _class.Graph.IamAvd = pCaptureDevice as IAMAnalogVideoDecoder;

            //Create user crossbar if needed
            if (_class.Var.UseCrossbar)
                if (_class.GraphCrossbar.createCrossbar(ref strCrossAudioOut, ref strCrossVideoOut, strCaptureVideoIn, strCaptureAudioIn, strShortName, pCaptureDevice))
                    _class.GraphCrossbar.checkCrossbar();

            _class.Debug.Log("");

            //Set resolution
            _class.Debug.Log("[0] Checking capture resolution");

            if (_class.Var.VideoResolutionIndex == 0 || _class.System.IsAutoSetCaptureResolution)
                _class.GraphResolution.Get();

            if (_class.Var.VideoResolutionIndex > 0)
                _class.GraphResolution.Set(pCaptureDevice, strCaptureVideoOut);
            else
                _class.Debug.Log("[0] [WARN] Cant find capture resolution - no input or unknown resolution type");

            var pRen = pCaptureDevice;
            var strPinOut = strCaptureVideoOut;
            var strDevice = strVideoDevice;

            //if (_class.Var.UseSampleGrabber)
            //    _class.SampleGrabber.createSampleGrabber(ref strPreviewIn, ref strPreviewOut, ref strDevice, ref strPinOut, ref pRen);

            if (_class.Var.CreateSmartTee)
            {
                _class.SmartTee.createSmartTee(ref strPreviewIn, ref strPreviewOut, ref strDevice, ref strPinOut, ref pRen);

                _class.Graph.CaptureFeed = pRen;
                _class.Graph.CaptureFeedIn = strPreviewIn;
            }

            IBaseFilter smartTeeBase = null;
            if (_class.System.IsVr)
                smartTeeBase = pRen;

            if (_class.Var.CreateAviRender)
                _class.AviRender.Create(ref strAvIin, ref strAvIout, ref strDevice, ref strPinOut, ref pRen);

            //Video renderer
            _class.Debug.Log("");
            _class.Debug.Log("[0]***   Create Video Renderer");
            Guid CLSID_ActiveVideo = new Guid("{B87BEB7B-8D29-423F-AE4D-6582C10175AC}");

            IBaseFilter pVideoRenderer = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_ActiveVideo));
            hr = _class.Graph.CaptureGraph.AddFilter(pVideoRenderer, "Video Renderer");
            if (hr == 0) { _class.Debug.Log("[1] [OK] Created video renderer"); }
            else
            {
                _class.Debug.Log("[1] [FAIL] Cant create video renderer");
                _class.Debug.Log("-> " + DsError.GetErrorText(hr));
            }

            _class.Debug.Log("");
            _class.Debug.Log("***   Listing Video Renderer pins");
            _class.GraphPin.ListPin(pVideoRenderer);
            strVideoIn = _class.GraphPin.AssumePinIn("Input");
            _class.Debug.Log("<Video>" + strVideoIn);
            _class.Debug.Log("");

            _class.Debug.Log("***   Connect AVI Decompressor (" + strPinOut + ") to Video Renderer (" + strVideoIn + ")");
            hr = _class.Graph.CaptureGraph.ConnectDirect(_class.GraphPin.GetPin(pRen, strPinOut), _class.GraphPin.GetPin(pVideoRenderer, strVideoIn), null);
            if (hr == 0) { _class.Debug.Log("[OK] Connected AVI to video renderer"); }
            else
            {
                _class.Debug.Log("[FAIL] Can't connect AVI to video renderer");
                _class.Debug.Log("-> " + DsError.GetErrorText(hr));
            }

            _class.Graph.VideoDef = pVideoRenderer as IBasicVideo;
            _class.Graph.VideoWindow = pVideoRenderer as IVideoWindow;

            if (_class.System.IsVr)
            {
                _class.Debug.Log("Create VR View");

                pRen = smartTeeBase;
                strPinOut = "Capture";

                IBaseFilter pAVIDecompressor2 = (IBaseFilter)new AVIDec();
                hr = _class.Graph.CaptureGraph.AddFilter(pAVIDecompressor2, "AVI Decompressor VR");
                _class.Debug.Log("-> " + DsError.GetErrorText(hr));

                hr = _class.Graph.CaptureGraph.ConnectDirect(_class.GraphPin.GetPin(pRen, strPinOut), _class.GraphPin.GetPin(pAVIDecompressor2, "XForm In"), null);
                _class.Debug.Log("-> " + DsError.GetErrorText(hr));

                IBaseFilter pVideoRenderer2 = (IBaseFilter)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_ActiveVideo));
                hr = _class.Graph.CaptureGraph.AddFilter(pVideoRenderer2, "Video Renderer VR");
                hr = _class.Graph.CaptureGraph.ConnectDirect(_class.GraphPin.GetPin(pAVIDecompressor2, "XForm Out"), _class.GraphPin.GetPin(pVideoRenderer2, "VMR Input0"), null);

                _class.Graph.VideoWindowVr = pVideoRenderer2 as IVideoWindow;
            }

            //Audio device
            if (_class.Audio.Output > -1 && _class.Audio.Output < _class.Audio.Devices.Count)
            {
                _class.Var.DeviceId = 0;               //Dont need multiple devices, set back to 0

                _class.Debug.Log("[0]");
                _class.Debug.Log("***   Create " + _class.Audio.Devices[_class.Audio.Output] + " audio device");
                IBaseFilter pAudio = null;

                pAudio = _class.GraphFilter.Set(FilterCategory.AudioRendererCategory, _class.Audio.Devices[_class.Audio.Output], out strTempOut);
                hr = _class.Graph.CaptureGraph.AddFilter(pAudio, "Audio Device");
                _class.Debug.Log("-> " + DsError.GetErrorText(hr));

                if (pAudio != null)
                {
                    _class.Debug.Log("[1]");
                    _class.Debug.Log("***   Listing " + _class.Audio.Devices[_class.Audio.Output] + " pins");

                    _class.GraphPin.ListPin(pAudio);
                    var strAudioIn = _class.GraphPin.AssumePinIn("Audio");
                    _class.Debug.Log("<Audio>" + strAudioIn);
                    _class.Debug.Log("");

                    //connect Capture Device and Audio Device
                    _class.Debug.Log("***   Connect " + strVideoDevice + " (" + strCaptureAudioOut + ") to " + _class.Audio.Devices[_class.Audio.Output] + " [Audio] (" + strAudioIn + ")");
                    hr = _class.Graph.CaptureGraph.ConnectDirect(_class.GraphPin.GetPin(pCaptureDevice, strCaptureAudioOut), _class.GraphPin.GetPin(pAudio, strAudioIn), null);
                    _class.Debug.Log("-> " + DsError.GetErrorText(hr));
                }
            }
            */
            return true;
        }

        /*
         * Looks for a unique identifier (eg Avermedia U3 extremecap will return Avermedia)
         * If there's more than one avermedia card, will look for the next available unique name
         * Finall, if there's two avermedia u3 on there, will still only return Avermedia
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

    }
}
