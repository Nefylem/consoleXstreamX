using System;
using System.Linq;
using consoleXstreamX.Configuration;
using consoleXstreamX.DisplayMenu.MainMenu;
using consoleXstreamX.Output;

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
            ShutterCommand.OkWait = MenuCommand.SetMoveWait() * 3;
            if (TitanOne.DeviceList.Count == 1)
            {
                Settings.UseTitanDevice = 0;
                if (TitanOne.DeviceList.Count > 0) Settings.TitanOneId = TitanOne.DeviceList[0].SerialNo;
                Settings.UseCronusMaxPlus = false;
                Settings.UseTitanOne = true;
                Settings.SaveConfiguration();
                ShowSelectedItems();
                return;
            }
            //Todo: come back to the previous menu from here
            MenuActions.SetMenu("TitanOne Output");
            MenuActions.ClearSubMenu();
            foreach (var item in TitanOne.DeviceList)
            {
                Shutter.AddItem($"TitanOne ({item.Id})", item.Id.ToString());
            }

            if (Settings.UseTitanDevice > TitanOne.DeviceList.Count) Settings.UseTitanDevice = 0;
            Shutter.CheckedItems.Add(Settings.UseTitanDevice.ToString());
            Shutter.Selected = Settings.UseTitanDevice.ToString();
        }

        public static void SelectTitanOne(string command)
        {
            int devId;
            int.TryParse(command, out devId);

            Settings.UseTitanDevice = devId;
            if (TitanOne.DeviceList.Count > devId) Settings.TitanOneId = TitanOne.DeviceList[devId].SerialNo;
            Settings.UseCronusMaxPlus = false;
            Settings.UseTitanOne = true;
            Settings.SaveConfiguration();

            Shutter.CheckedItems.Clear();
            Shutter.CheckedItems.Add(Settings.UseTitanDevice.ToString());
        }
    }
}
