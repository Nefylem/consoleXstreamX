using System;
using System.Drawing;
using System.Windows.Forms;
using consoleXstreamX.Capture;
using consoleXstreamX.Configuration;
using consoleXstreamX.Debugging;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Input;
using consoleXstreamX.Input.Keyboard;
using consoleXstreamX.Resolution;

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
            timer.Enabled = true;
        }

        private void Setup()
        {
            //Todo: get the starting display resolution so we can swap back to it when exiting
            new Logging().Cleanup();
            Shortcuts.Load();
            SetWindow();
            KeyHook.Enable();
            VideoCapture.Startup(this);
        }

        private void SetWindow()
        {
            //Todo: check between fullscreen modes
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
            Activate();

            display.Dock = DockStyle.Fill;
            display.BackColor = Color.Black;

            wait.Dock = DockStyle.Fill;
            wait.BackColor = Color.Aqua;
        }

        public void FocusWindow()
        {
            if (wait.Visible)
            {
                display.Visible = true;
                wait.Visible = false;
            }
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
            if (MenuController.Shutdown) CloseApplication();
            CheckResolution();
            CheckControllerInput();
        }

        private void CheckResolution()
        {
            var height = 0;
            if (Settings.AutoSetCaptureResolution) height = VideoCapture.CheckResolution();
            if (Settings.AutoSetDisplayResolution && height != 0) DisplayResolution.Change(height);
        }

        private void CheckControllerInput()
        {
            var input = Gamepad.Check(1);
            if (KeyHook.GetKey("ESCAPE")) MenuController.Open();
        }

        private void CloseApplication()
        {
            timer.Enabled = false;
            VideoCapture.CloseGraph();
            Application.DoEvents();
            Application.Exit();
        }

        public void SetFullscreen()
        {
            if (Settings.Fullscreen)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                Bounds = Screen.PrimaryScreen.Bounds;
                Activate();
            }
        }
    }
}
