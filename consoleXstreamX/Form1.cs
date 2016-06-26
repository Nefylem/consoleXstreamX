using System;
using System.Drawing;
using System.Windows.Forms;
using consoleXstreamX.Capture;
using consoleXstreamX.Configuration;
using consoleXstreamX.Debugging;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Input;
using consoleXstreamX.Input.Keyboard;

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
            if (Settings.AutoSetCaptureResolution) VideoCapture.CheckResolution();
            CheckControllerInput();
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
