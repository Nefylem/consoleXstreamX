using System;
using System.Drawing;
using System.Windows.Forms;
using consoleXstreamX.Capture;
using consoleXstreamX.Configuration;
using consoleXstreamX.Debugging;
using consoleXstreamX.Display;
using consoleXstreamX.DisplayMenu;
using consoleXstreamX.Input;
using consoleXstreamX.Input.Keyboard;
using consoleXstreamX.Output;
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
        }

        private void Setup()
        {
            new Logging().Cleanup();
            Settings.LoadConfiguration();
            Shortcuts.Load();
            SetWindow();
            KeyHook.Enable();
            if (Settings.AllowCronusMaxPlus) CronusmaxPlus.Open();
            if (Settings.AllowTitanOne)
            {
                TitanOne.Open();
                TitanOne.FindDevices();
            }
            VideoCapture.Startup(this);
            system.Enabled = true;
            controlPrimary.Enabled = true;
            controlSecondary.Enabled = true;
        }

        private void SetWindow()
        {
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
            Activate();
            /*
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Activate();
            */
            display.Dock = DockStyle.Fill;
            display.BackColor = Color.Black;

            wait.Dock = DockStyle.Fill;
            wait.BackColor = Color.Aqua;

            if (Settings.ControlScreenSaver) ScreenSaver.DisableScreenSaver();
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


        private void CheckResolution()
        {
            var height = 0;
            if (Settings.AutoSetCaptureResolution) height = VideoCapture.CheckResolution();
            //if (Settings.AutoSetDisplayResolution && height != 0) DisplayResolution.Change(height);
        }

        private void CheckControllerInput()
        {
            var input = Gamepad.Check(1);
            if (Settings.UseCronusMaxPlus) CronusmaxPlus.Send(input);
            if (Settings.UseTitanOne) TitanOne.Send(input);
            /*
            if (Settings.EnableKeyboard && !MenuController.Visible)
            {
                if (Settings.UseCronusMaxPlus) CronusmaxPlus.Send(KeyboardInterface.Read());
                if (Settings.UseTitanOne) TitanOne.Send(KeyboardInterface.Read());
            }
            */
            if (KeyHook.GetKey("ESCAPE")) MenuController.Open();
        }

        private void CloseApplication()
        {
            system.Enabled = false;
            controlPrimary.Enabled = false;
            controlSecondary.Enabled = false;

            Application.DoEvents();
            VideoCapture.CloseGraph();

            if (Settings.AllowCronusMaxPlus) CronusmaxPlus.Close();
            if (Settings.AllowTitanOne) TitanOne.Close();
            if (Settings.ControlScreenSaver) ScreenSaver.EnableScreenSaver();

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

        private void system_Tick(object sender, EventArgs e)
        {
            //Run system control tasks only. Let the primary and secondary control methods handle the inputs
            if (MenuController.Shutdown) CloseApplication();
            CheckResolution();
        }

        private void controlPrimary_Tick(object sender, EventArgs e)
        {
            CheckControllerInput();
        }

        private void controlSecondary_Tick(object sender, EventArgs e)
        {
            //Check for key input
            CheckControllerInput();
        }
    }
}
