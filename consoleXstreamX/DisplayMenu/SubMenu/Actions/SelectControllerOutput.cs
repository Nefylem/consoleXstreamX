using System;
using System.Linq;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectControllerOutput
    {
        public static void Show()
        {
            MenuActions.SetMenu("Controller Output");
            MenuActions.ClearSubMenu();

            if (Settings.AllowCronusMaxPlus) Shutter.AddItem("CronusMaxPlus", "CronusMaxPlus");
            if (Settings.AllowTitanOne) Shutter.AddItem("TitanOne", "TitanOne");
            if (Settings.AllowGimx) Shutter.AddItem("Remote GIMX", "Remote GIMX");

            ShowSelectedItems();

            var record = Shutter.Tiles.FirstOrDefault();
            if (record != null) Shutter.Selected = record.Command;
            Shutter.SetActiveRow(-1);
        }

        private static void ShowSelectedItems()
        {
            Shutter.CheckedItems.Clear();
            if (Settings.UseCronusMaxPlus) Shutter.CheckedItems.Add("CronusMaxPlus");
            if (Settings.UseTitanOne) Shutter.CheckedItems.Add("TitanOne");
            if (Settings.UseGimxRemote) Shutter.CheckedItems.Add("Remote GIMX");
        }

        public static void Select(string item)
        {
            if (string.Equals(item, "CronusMaxPlus", StringComparison.CurrentCultureIgnoreCase)) SetCronusMaxPlus();
            if (string.Equals(item, "TitanOne", StringComparison.CurrentCultureIgnoreCase)) SetTitanOne();
        }

        private static void SetCronusMaxPlus()
        {
            Settings.UseTitanOne = false;
            Settings.UseCronusMaxPlus = true;
            Settings.SaveConfiguration();
            ShowSelectedItems();
        }

        private static void SetTitanOne()
        {
            //Todo: select which titanone to use
            Settings.UseCronusMaxPlus = false;
            Settings.UseTitanOne = true;
            Settings.SaveConfiguration();
            ShowSelectedItems();
        }
    }
}
