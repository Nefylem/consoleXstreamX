﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace consoleXstreamX.DisplayMenu
{
    public static class MenuController
    {
        private static readonly MenuMain Menu;

        public static bool Visible;
        public static int Delay;
        public static int DelayLimit;

        public static bool Shutdown;

        static MenuController()
        {
            Menu = new MenuMain();
            DelayLimit = 80;
        }

        public static void Open()
        {
            if (Visible) return;
            Visible = true;
            Menu.ShowPanel();
        }

        public static void Close()
        {
            if (!Visible) return;
            Visible = false;
            Menu.ClosePanel();
        }

        public static void ExitApplication()
        {
            Menu.ClosePanel();
            Shutdown = true;
        }
    }
}
