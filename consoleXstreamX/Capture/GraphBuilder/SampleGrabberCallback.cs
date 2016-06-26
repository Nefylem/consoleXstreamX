using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib;

namespace consoleXstreamX.Capture.GraphBuilder
{
    class SampleGrabberCallback : ISampleGrabberCB
    {
        private Form1 _form1;
        private string _strFpsCheck;
        private int _intSampleFrame;
        private int _numberOfLines;

        public void GetForm1Handle(Form1 inForm) { _form1 = inForm; }

        public int BufferCB(double sampleTime, IntPtr pBuffer, int bufferLen)
        {
            if (_strFpsCheck != DateTime.Now.ToString("ss"))
            {
                _form1.SampleFps = _intSampleFrame;
                _intSampleFrame = 0;
                _strFpsCheck = DateTime.Now.ToString("ss");
            }
            else
            {
                _intSampleFrame++;
            }

            /*
            if (VideoCapture.IamAvd == null) return 0;
            int lineCount;
            VideoCapture.IamAvd.get_NumberOfLines(out lineCount);
            if (_numberOfLines == 0) _numberOfLines = lineCount;
            if (_numberOfLines == lineCount) return 0;

            _numberOfLines = lineCount;
            ResolutionController.ChangeResolution(lineCount);
            */
            return 0;
        }

        public int SampleCB(double sampleTime, IMediaSample pSample)
        {
            return 0;
        }

    }
}
