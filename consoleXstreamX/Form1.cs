using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using consoleXstreamX.Capture;
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
            button1.BringToFront();
            /*
            */
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
            //CheckControllerInput();
        }

        private void CheckControllerInput()
        {
            var input = Gamepad.Check(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //VideoCapture.SetWait();
            MenuController.Open();
        }

        private void CloseApplication()
        {
            timer.Enabled = false;
            VideoCapture.CloseGraph();
            Application.DoEvents();
            Application.Exit();
        }
    }
}
