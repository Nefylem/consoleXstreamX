using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using consoleXstreamX.Capture;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectVideoDevice
    {
        public static void Show()
        {
            MenuActions.SetMenu("Video Device");
            MenuActions.ClearSubMenu();

            if (VideoCapture.CaptureDevices.Count > 0)
            {
                foreach (var item in VideoCapture.CaptureDevices)
                {
                    Shutter.AddItem(item.Title, item.Title);
                }
                var record = VideoCapture.CaptureDevices.FirstOrDefault();
                if (record != null) Shutter.Selected = record.Title;
            }
            else
            {
                Shutter.Error = "No capture devices found";
                Shutter.Explanation = "";
            }
            Shutter.SetActiveRow(1);
        }
    }
}
