using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using consoleXstreamX.Capture.Analyse;
using consoleXstreamX.Capture.Settings;

namespace consoleXstreamX.Capture
{
    class Startup
    {
        public void Execute()
        {
            InitializeCapture();
            new GraphBuilder.Startup().RunGraph();
        }

        private void InitializeCapture()
        {
            VideoCapture.CrossbarAudio = "";
            VideoCapture.CrossbarVideo = "";

            new VideoInput().Find();
            new AudioRenderer().Find();
            new UserSettings().Read();          
            new CaptureResolution().List();

            VideoCapture.InitializeGraph = true;
            VideoCapture.RestartGraph = true;
        }

    }
}
