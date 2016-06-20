using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class Pin
    {
        public PinList List(IBaseFilter filter)
        {
            var results = new PinList
            {
                Out = new List<string>(),
                In = new List<string>()
            };

            FilterInfo pOut;
            filter.QueryFilterInfo(out pOut);
            Debug.Log("[3] listing Pins [" + pOut.achName + "]");

            try
            {
                IEnumPins epins;
                var hr = filter.EnumPins(out epins);
                if (hr < 0)
                {
                    Debug.Log("[0] [NG] Can't find pins");
                    return results;
                }
                var fetched = Marshal.AllocCoTaskMem(4);
                var pins = new IPin[1];
                while (epins.Next(1, pins, fetched) == 0)
                {
                    PinInfo pinfo;
                    pins[0].QueryPinInfo(out pinfo);

                    if (pinfo.dir.ToString().ToLower() == "input") { results.In.Add(pinfo.name); }
                    if (pinfo.dir.ToString().ToLower() == "output") { results.Out.Add(pinfo.name); }

                    DsUtils.FreePinInfo(pinfo);
                }

                LogPins(results.In, "Input");
                LogPins(results.Out, "Output");
            }
            catch
            {
                Debug.Log("[0] [FAIL] Error listing pins");
            }
            return results;
        }

        public IPin Get(IBaseFilter filter, string pinname)
        {
            IEnumPins epins;
            if (filter == null) return null;
            var hr = filter.EnumPins(out epins);

            var fetched = Marshal.AllocCoTaskMem(4);
            var pins = new IPin[1];
            while (epins.Next(1, pins, fetched) == 0)
            {
                PinInfo pinfo;
                pins[0].QueryPinInfo(out pinfo);
                var found = (pinfo.name == pinname);
                DsUtils.FreePinInfo(pinfo);
                if (found) return pins[0];
            }
            return null;
        }

        public string Assume(string search, List<string> list)
        {
            foreach (var item in list)
            {
                if (item.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) > -1) return item;
            }
            return "";
            //var index = list.FindIndex(x => x.Equals(search, StringComparison.CurrentCultureIgnoreCase));
            //return index > -1 ? list[index] : "";
        }

        public string Assume(string search, string type, List<string> list)
        {
            if (string.Equals(type, "vídeo", StringComparison.CurrentCultureIgnoreCase)) type = "video";

            foreach (var item in list)
            {
                if (item.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) == -1) continue;
                if (string.Equals(type, "video", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (item.IndexOf("audio", StringComparison.CurrentCultureIgnoreCase) == -1) return item;
                }
                else
                {
                    if (item.IndexOf("video", StringComparison.CurrentCultureIgnoreCase) == -1) return item;
                }
            }
            return "";
        }

        private void LogPins(List<string> pins, string direction)
        {
            foreach (var item in pins)
            {
                Debug.Log($"- {item} ({direction})");
            }
        }

        public class PinList
        {
            public List<string> In;
            public List<string> Out;
        }

        public class CapturePins
        {
            public Node Video;
            public Node Audio;
        }

        public class Node
        {
            public string In;
            public string Out;
        }

    }
}
