using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class VideoConnectors
    {
        public Pin.CapturePins Make(IBaseFilter pCaptureDevice)
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


    }
}
