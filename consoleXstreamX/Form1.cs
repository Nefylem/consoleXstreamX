using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using consoleXstreamX.Capture;
using consoleXstreamX.Debugging;
using consoleXstreamX.Define;
using consoleXstreamX.Input;

namespace consoleXstreamX
{
    public partial class Form1 : Form
    {
        public int SampleFps;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Setup();
            /*
            VideoCapture.Startup(this);
            */
            //timer.Enabled = true;
        }

        private void Setup()
        {
            new Logging().Cleanup();
            Shortcuts.Load();
            SetWindow();
        }

        private void SetWindow()
        {
            if (!Debugger.IsAttached)
            {
                //Todo: check between fullscreen modes
                WindowState = FormWindowState.Normal;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Bounds = Screen.PrimaryScreen.Bounds;
                Activate();
            }

            display.Dock = DockStyle.Fill;
            display.BackColor = Color.Black;
        }

        public void FocusWindow()
        {
            display.BringToFront();
            display.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            display.Dock = DockStyle.Fill;
            VideoCapture.ResetDisplay();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            CheckControllerInput();
        }

        private void CheckControllerInput()
        {
            var input = Gamepad.Check(1);
        }
    }
}
