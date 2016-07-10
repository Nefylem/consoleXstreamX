using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectControllerSettings
    {
        public static void Show()
        {
            MenuActions.SetMenu("Controller Settings");
            MenuActions.ClearSubMenu();

            Shutter.AddItem("Allow Passthrough", "Allow Passthrough");
            Shutter.AddItem("Normalize", "Normalize");
            Shutter.AddItem("DS4 Emulation", "DS4 Emulation");
            Shutter.AddItem("Rumble", "Rumble");
            Shutter.AddItem("Use Calibration", "Use Calibration");
            Shutter.AddItem("Calibrate", "Calibrate");

            SetValue();

            var record = Shutter.Tiles.FirstOrDefault();
            if (record != null) Shutter.Selected = record.Command;
            Shutter.SetActiveRow(-1);
        }

        private static void SetValue()
        {
            Shutter.CheckedItems.Clear();
            if (Settings.AllowPassthrough) Shutter.CheckedItems.Add("Allow Passthrough");
            if (Settings.Ds4ControllerMode) Shutter.CheckedItems.Add("DS4 Emulation");
            if (Settings.NormalizeControls) Shutter.CheckedItems.Add("Normalize");
            if (Settings.Rumble) Shutter.CheckedItems.Add("Rumble");
            if (Settings.Rumble) Shutter.CheckedItems.Add("Use Calibration");
        }

        public static void Execute(string command)
        {
            if (string.Equals(command, "normalize", StringComparison.CurrentCultureIgnoreCase)) { Settings.NormalizeControls = !Settings.NormalizeControls; SetValue(); Settings.SaveConfiguration(); }
            if (string.Equals(command, "ds4 emulation", StringComparison.CurrentCultureIgnoreCase)) { Settings.Ds4ControllerMode = !Settings.Ds4ControllerMode; SetValue(); Settings.SaveConfiguration(); }
            if (string.Equals(command, "rumble", StringComparison.CurrentCultureIgnoreCase)) { Settings.Rumble = !Settings.Rumble; SetValue(); Settings.SaveConfiguration(); }
            if (string.Equals(command, "use rumble", StringComparison.CurrentCultureIgnoreCase)) { Settings.UseCalibration = !Settings.UseCalibration; SetValue(); Settings.SaveConfiguration(); }
            if (string.Equals(command, "calibrate", StringComparison.CurrentCultureIgnoreCase))
            {
                SetCalibrationMode();
            }
        }

        private static void SetCalibrationMode()
        {
            MenuActions.SetMenu("Controller Calibration");
            MenuActions.ClearSubMenu();
            Shutter.Error = "Controller Calibration";
            Shutter.Explanation = "Move thumbsticks in circles several times. Back to finish";
            MenuSettings.CalibrateController = true;
        }
    }
}
