using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;
using consoleXstreamX.Drawing;

namespace consoleXstreamX.DisplayMenu
{
    public partial class MenuMain : Form
    {
        //Hide from Alt-Tab / Task Manager
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        public MenuMain()
        {
            InitializeComponent();
        }

        private void MenuMain_Load(object sender, EventArgs e)
        {

        }

        public void ShowPanel()
        {
            MenuSettings.History = new List<string>();
            MainCommands.GetMainMenuItems();
                                 
            TransparencyKey = Color.FromArgb(255, 1, 1, 1);
            BackColor = Color.FromArgb(255, 1, 1, 1);

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;

            display.Dock = DockStyle.Fill;
            //Make as close to invisible as possible, to stop the menu "flashing" into view
            Width = 1;
            Height = 1;
            Opacity = 0.1;

            if (Settings.StayOnTop) TopMost = true;
            Show();
            PositionMenu();

            menuTimer.Enabled = true;
        }

        private void PositionMenu()
        {
            Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (Properties.Resources.main_menu_back.Width / 2);
            Top = (Screen.PrimaryScreen.Bounds.Height / 2) - (Properties.Resources.main_menu_back.Height / 2);

            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Properties.Resources.main_menu_back.Height;
            //Todo: nice face in
            Opacity = 0.90;
        }

        private void menuTimer_Tick(object sender, EventArgs e)
        {
            DrawPanel();
        }

        private void DrawPanel()
        {
            Draw.ClearGraph(Properties.Resources.main_menu_back);
            DrawMain.Execute();

            /*

            if (!_class.Var.IsMainMenu)
            {
                _class.SubMenu.Draw();
                if (_class.Var.ShowSubSelection) _class.SubSelectMenu.Draw();
            }

            _class.DrawGui.setOutline(false);

            if (Settings.CheckFps)
                _class.DrawGui.drawText(5, 500, "FPS: " + _class.Fps.Frames);

            */
            display.Image = Draw.GetImage();

        }
    }
}
