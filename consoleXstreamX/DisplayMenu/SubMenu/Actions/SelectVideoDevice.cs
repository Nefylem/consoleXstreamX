using System.Linq;
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

        public static void Execute(string command)
        {
            //var record = VideoCapture.CaptureDevices.FirstOrDefault(s => s.Title == command);
            var index = VideoCapture.CaptureDevices.FindIndex(s => s.Title == command);
            if (index == -1) return;
            VideoCapture.CurrentVideoDevice = index;
            VideoCapture.RunGraph();
        }
    }
}
