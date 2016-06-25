using System;
using consoleXstreamX.DisplayMenu.MainMenu;

namespace consoleXstreamX.DisplayMenu.SubMenu.Actions
{
    internal static class SelectExit
    {
        public static void Show()
        {
            MenuActions.SetMenu("Exit");
            MenuActions.ClearSubMenu();

            Shutter.AddItem("Yes", "exit");
            Shutter.AddItem("No", "back");

            Shutter.Selected = "back";
            Shutter.SetActiveRow(-1);
        }

        public static void Execute(string command)
        {
            if (string.Equals(command, "Back", StringComparison.CurrentCultureIgnoreCase)) ShutterCommand.StepBackMenu();
            if (string.Equals(command, "Exit", StringComparison.CurrentCultureIgnoreCase))
            {
                MenuController.ExitApplication();
            }
        }
    }
}
