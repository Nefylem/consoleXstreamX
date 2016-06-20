using System.Runtime.InteropServices;
using consoleXstreamX.Debugging;
using DirectShowLib;

namespace consoleXstreamX.Capture.Analyse
{
    class CaptureResolution
    {
        public void List()
        {
            Debug.Log("[0] Finding resolutions for Video Capture Devices");
            for (var count = 0; count < VideoCapture.CaptureDevices.Count; count++)
            {
                var device = VideoCapture.CaptureDevices[count];
                Debug.Log($"[1] -= Viewing {device.Title} =-");

                var dev = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice)[count];
                var filterGraph = (IFilterGraph3)new FilterGraphNoThread();
                IBaseFilter baseDev;

                filterGraph.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out baseDev);
                var pin = DsFindPin.ByCategory(baseDev, PinCategory.Capture, 0);
                var streamConfig = (IAMStreamConfig)pin;
                int iC, iS;
                streamConfig.GetNumberOfCapabilities(out iC, out iS);
                var ptr = Marshal.AllocCoTaskMem(iS);

                for (var i = 0; i < iC; i++)
                {
                    AMMediaType media;
                    streamConfig.GetStreamCaps(i, out media, ptr);
                    var v = new VideoInfoHeader();
                    Marshal.PtrToStructure(media.formatPtr, v);

                    if (v.BmiHeader.Width == 0) continue;
                    var type = CheckMediaType(media);
                    device.Resolution.Add(new VideoCapture.VideoCaptureResolution()
                    {
                        Height = v.BmiHeader.Height,
                        Width = v.BmiHeader.Width,
                        Type = type,
                        MediaType = media
                    });
                    Debug.Log($"[2] {v.BmiHeader.Width} x {v.BmiHeader.Height} ({type}");
                }
            }
        }

        private static string CheckMediaType(AMMediaType media)
        {
            if (media.subType == MediaSubType.A2B10G10R10) { return "A2B10G10R10"; }
            if (media.subType == MediaSubType.A2R10G10B10) { return "A2R10G10B10"; }
            if (media.subType == MediaSubType.AI44) { return "AI44"; }
            if (media.subType == MediaSubType.AIFF) { return "AI44"; }
            if (media.subType == MediaSubType.AnalogVideo_NTSC_M) { return "AnalogVideo_NTSC_M"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_B) { return "AnalogVideo_PAL_B"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_D) { return "AnalogVideo_PAL_D"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_G) { return "AnalogVideo_PAL_G"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_H) { return "AnalogVideo_PAL_H"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_I) { return "AnalogVideo_PAL_I"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_M) { return "AnalogVideo_PAL_M"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_N) { return "AnalogVideo_PAL_N"; }
            if (media.subType == MediaSubType.AnalogVideo_PAL_N_COMBO) { return "AnalogVideo_PAL_N_COMBO"; }
            if (media.subType == MediaSubType.AnalogVideo_SECAM_B) { return "AnalogVideo_SECAM_B"; }
            if (media.subType == MediaSubType.AnalogVideo_SECAM_D) { return "AnalogVideo_SECAM_D"; }
            if (media.subType == MediaSubType.AnalogVideo_SECAM_G) { return "AnalogVideo_SECAM_G"; }
            if (media.subType == MediaSubType.AnalogVideo_SECAM_H) { return "AnalogVideo_SECAM_H"; }
            if (media.subType == MediaSubType.AnalogVideo_SECAM_K) { return "AnalogVideo_SECAM_K"; }
            if (media.subType == MediaSubType.AnalogVideo_SECAM_K1) { return "AnalogVideo_SECAM_K1"; }
            if (media.subType == MediaSubType.AnalogVideo_SECAM_L) { return "AnalogVideo_SECAM_L"; }
            if (media.subType == MediaSubType.ARGB1555) { return "ARGB1555"; }
            if (media.subType == MediaSubType.ARGB1555_D3D_DX7_RT) { return "ARGB1555_D3D_DX7_RT"; }
            if (media.subType == MediaSubType.ARGB1555_D3D_DX9_RT) { return "ARGB1555_D3D_DX9_RT"; }
            if (media.subType == MediaSubType.ARGB32) { return "ARGB32"; }
            if (media.subType == MediaSubType.ARGB32_D3D_DX7_RT) { return "ARGB32_D3D_DX7_RT"; }
            if (media.subType == MediaSubType.ARGB32_D3D_DX9_RT) { return "ARGB32_D3D_DX9_RT"; }
            if (media.subType == MediaSubType.ARGB4444) { return "ARGB4444"; }
            if (media.subType == MediaSubType.ARGB4444_D3D_DX7_RT) { return "ARGB4444_D3D_DX7_RT"; }
            if (media.subType == MediaSubType.ARGB4444_D3D_DX9_RT) { return "ARGB4444_D3D_DX9_RT"; }
            if (media.subType == MediaSubType.Asf) { return "Asf"; }
            if (media.subType == MediaSubType.AtscSI) { return "AtscSI"; }
            if (media.subType == MediaSubType.AU) { return "AU"; }
            if (media.subType == MediaSubType.Avi) { return "Avi"; }
            if (media.subType == MediaSubType.AYUV) { return "AYUV"; }
            if (media.subType == MediaSubType.CFCC) { return "CFCC"; }
            if (media.subType == MediaSubType.CLJR) { return "CLJR"; }
            if (media.subType == MediaSubType.CPLA) { return "CPLA"; }
            if (media.subType == MediaSubType.Data708_608) { return "Data708_608"; }
            if (media.subType == MediaSubType.DOLBY_AC3_SPDIF) { return "DOLBY_AC3_SPDIF"; }
            if (media.subType == MediaSubType.DolbyAC3) { return "DolbyAC3"; }
            if (media.subType == MediaSubType.DRM_Audio) { return "DRM_Audio"; }
            if (media.subType == MediaSubType.DssAudio) { return "DssAudio"; }
            if (media.subType == MediaSubType.DssVideo) { return "DssVideo"; }
            if (media.subType == MediaSubType.DtvCcData) { return "DtvCcData"; }
            if (media.subType == MediaSubType.dv25) { return "dv25"; }
            if (media.subType == MediaSubType.dv50) { return "dv50"; }
            if (media.subType == MediaSubType.DvbSI) { return "DvbSI"; }
            if (media.subType == MediaSubType.DVCS) { return "DVCS"; }
            if (media.subType == MediaSubType.dvh1) { return "dvh1"; }
            if (media.subType == MediaSubType.dvhd) { return "dvhd"; }
            if (media.subType == MediaSubType.DVSD) { return "DVSD"; }
            if (media.subType == MediaSubType.dvsl) { return "dvsl"; }
            if (media.subType == MediaSubType.H264) { return "H264"; }
            if (media.subType == MediaSubType.I420) { return "I420"; }
            if (media.subType == MediaSubType.IA44) { return "IA44"; }
            if (media.subType == MediaSubType.IEEE_FLOAT) { return "IEEE_FLOAT"; }
            if (media.subType == MediaSubType.IF09) { return "IF09"; }
            if (media.subType == MediaSubType.IJPG) { return "IJPG"; }
            if (media.subType == MediaSubType.IMC1) { return "IMC1"; }
            if (media.subType == MediaSubType.IMC2) { return "IMC2"; }
            if (media.subType == MediaSubType.IMC3) { return "IMC3"; }
            if (media.subType == MediaSubType.IMC4) { return "IMC4"; }
            if (media.subType == MediaSubType.IYUV) { return "IYUV"; }
            if (media.subType == MediaSubType.Line21_BytePair) { return "Line21_BytePair"; }
            if (media.subType == MediaSubType.Line21_GOPPacket) { return "Line21_GOPPacket"; }
            if (media.subType == MediaSubType.Line21_VBIRawData) { return "Line21_VBIRawData"; }
            if (media.subType == MediaSubType.MDVF) { return "MDVF"; }
            if (media.subType == MediaSubType.MJPG) { return "MJPG"; }
            if (media.subType == MediaSubType.MPEG1AudioPayload) { return "MPEG1AudioPayload"; }
            if (media.subType == MediaSubType.MPEG1Packet) { return "MPEG1Packet"; }
            if (media.subType == MediaSubType.MPEG1Payload) { return "MPEG1Payload"; }
            if (media.subType == MediaSubType.MPEG1System) { return "MPEG1System"; }
            if (media.subType == MediaSubType.MPEG1SystemStream) { return "MPEG1SystemStream"; }
            if (media.subType == MediaSubType.MPEG1Video) { return "MPEG1Video"; }
            if (media.subType == MediaSubType.MPEG1VideoCD) { return "MPEG1VideoCD"; }
            if (media.subType == MediaSubType.Mpeg2Audio) { return "Mpeg2Audio"; }
            if (media.subType == MediaSubType.Mpeg2Data) { return "Mpeg2Data"; }
            if (media.subType == MediaSubType.Mpeg2Program) { return "Mpeg2Program"; }
            if (media.subType == MediaSubType.Mpeg2Transport) { return "Mpeg2Transport"; }
            if (media.subType == MediaSubType.Mpeg2TransportStride) { return "Mpeg2TransportStride"; }
            if (media.subType == MediaSubType.Mpeg2Video) { return "Mpeg2Video"; }
            if (media.subType == MediaSubType.None) { return "None"; }
            if (media.subType == MediaSubType.Null) { return "Null"; }
            if (media.subType == MediaSubType.NV12) { return "NV12"; }
            if (media.subType == MediaSubType.NV24) { return "NV24"; }
            if (media.subType == MediaSubType.Overlay) { return "Overlay"; }
            if (media.subType == MediaSubType.PCM) { return "PCM"; }
            if (media.subType == MediaSubType.PCMAudio_Obsolete) { return "PCMAudio_Obsolete"; }
            if (media.subType == MediaSubType.PLUM) { return "PLUM"; }
            if (media.subType == MediaSubType.QTJpeg) { return "QTJpeg"; }
            if (media.subType == MediaSubType.QTMovie) { return "QTMovie"; }
            if (media.subType == MediaSubType.QTRle) { return "QTRle"; }
            if (media.subType == MediaSubType.QTRpza) { return "QTRpza"; }
            if (media.subType == MediaSubType.QTSmc) { return "QTSmc"; }
            if (media.subType == MediaSubType.RAW_SPORT) { return "RAW_SPORT"; }
            if (media.subType == MediaSubType.RGB1) { return "RGB1"; }
            if (media.subType == MediaSubType.RGB16_D3D_DX7_RT) { return "RGB16_D3D_DX7_RT"; }
            if (media.subType == MediaSubType.RGB16_D3D_DX9_RT) { return "RGB16_D3D_DX9_RT"; }
            if (media.subType == MediaSubType.RGB24) { return "RGB24"; }
            if (media.subType == MediaSubType.RGB32) { return "RGB32"; }
            if (media.subType == MediaSubType.RGB32_D3D_DX7_RT) { return "RGB32_D3D_DX7_RT"; }
            if (media.subType == MediaSubType.RGB32_D3D_DX9_RT) { return "RGB32_D3D_DX9_RT"; }
            if (media.subType == MediaSubType.RGB4) { return "RGB4"; }
            if (media.subType == MediaSubType.RGB555) { return "RGB555"; }
            if (media.subType == MediaSubType.RGB565) { return "RGB565"; }
            if (media.subType == MediaSubType.RGB8) { return "RGB8"; }
            if (media.subType == MediaSubType.S340) { return "S340"; }
            if (media.subType == MediaSubType.S342) { return "S342"; }
            if (media.subType == MediaSubType.SPDIF_TAG_241h) { return "SPDIF_TAG_241h"; }
            if (media.subType == MediaSubType.TELETEXT) { return "TELETEXT"; }
            if (media.subType == MediaSubType.TVMJ) { return "TVMJ"; }
            if (media.subType == MediaSubType.UYVY) { return "UYVY"; }
            if (media.subType == MediaSubType.VideoImage) { return "VideoImage"; }
            if (media.subType == MediaSubType.VPS) { return "VPS"; }
            if (media.subType == MediaSubType.VPVBI) { return "VPVBI"; }
            if (media.subType == MediaSubType.VPVideo) { return "VPVideo"; }
            if (media.subType == MediaSubType.WAKE) { return "WAKE"; }
            if (media.subType == MediaSubType.WAVE) { return "WAVE"; }
            if (media.subType == MediaSubType.WebStream) { return "WebStream"; }
            if (media.subType == MediaSubType.WSS) { return "WSS"; }
            if (media.subType == MediaSubType.Y211) { return "Y211"; }
            if (media.subType == MediaSubType.Y411) { return "Y411"; }
            if (media.subType == MediaSubType.Y41P) { return "Y41P"; }
            if (media.subType == MediaSubType.YUY2) { return "YUY2"; }
            if (media.subType == MediaSubType.YUYV) { return "YUYV"; }
            if (media.subType == MediaSubType.YV12) { return "YV12"; }
            if (media.subType == MediaSubType.YVU9) { return "YVU9"; }
            if (media.subType == MediaSubType.YVYU) { return "YVYU"; }

            return "";
        }

    }
}
