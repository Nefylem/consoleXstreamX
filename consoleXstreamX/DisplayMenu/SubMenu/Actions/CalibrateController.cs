using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Configuration;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class CalibrateController
    {
        public static void Calculate()
        {
            Settings.LeftXMinRatio = -1.0/MenuSettings.MinLeftX;
            Settings.LeftXMaxRatio = 1.0/MenuSettings.MaxLeftX;
            Settings.LeftYMinRatio = -1.0/MenuSettings.MinLeftY;
            Settings.LeftYMaxRatio = 1.0 /MenuSettings.MaxLeftY;

            Settings.RightXMinRatio = -1.0/MenuSettings.MinRightX;
            Settings.RightXMaxRatio = 1.0 /MenuSettings.MaxRightX;
            Settings.RightYMinRatio = -1.0 /MenuSettings.MinRightY;
            Settings.RightYMaxRatio = 1.0 /MenuSettings.MaxRightY;

            Settings.SaveConfiguration();
        }
    }
}
