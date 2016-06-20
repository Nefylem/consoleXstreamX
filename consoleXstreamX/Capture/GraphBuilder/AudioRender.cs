using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class AudioRender
    {
        public void Create(string pAudioOut, IBaseFilter pCaptureDevice)
        {
            var pin = new Pin();
            var filter = new Filter();
            if (VideoCapture.CurrentAudioDevice <= -1 || (VideoCapture.CurrentAudioDevice >= VideoCapture.AudioDevices.Count)) return;
            var device = VideoCapture.AudioDevices[VideoCapture.CurrentAudioDevice];
            VideoCapture.CurrentActiveDevice = 0;           //Dont need multiple devices, set back to 0
            Debug.Log("[0]");
            Debug.Log($"***   Create {device} audio device");

            string temp;
            var pAudio = filter.Set(FilterCategory.AudioRendererCategory, device, out temp);
            var hr = VideoCapture.CaptureGraph.AddFilter(pAudio, "Audio Device");
            Debug.Log("-> " + DsError.GetErrorText(hr));

            if (pAudio == null) return;
            Debug.Log("[1]");
            Debug.Log($"***   Listing {device} pins");

            var pinList = pin.List(pAudio);
            var audioIn = pin.Assume("Audio", pinList.In);
            Debug.Log($"<Audio> {audioIn}");
            Debug.Log("");

            //connect Capture Device and Audio Device
            Debug.Log($"***   Connect {VideoCapture.CurrentVideo} ({pAudioOut}) to {device} [Audio] ({audioIn})");
            hr = VideoCapture.CaptureGraph.ConnectDirect(pin.Get(pCaptureDevice, pAudioOut), pin.Get(pAudio, audioIn), null);
            Debug.Log("-> " + DsError.GetErrorText(hr));
        }
    }
}
