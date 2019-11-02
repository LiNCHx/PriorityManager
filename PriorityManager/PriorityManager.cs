using PriorityManager.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriorityManager
{
    public class PriorityManager : ApplicationContext
    {

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        private MenuItem programList;
        private MenuItem savedList;

        private Utils utils = new Utils();
        private Task updateTask;

        public PriorityManager()
        {
            programList = new MenuItem("Programs");
            savedList = new MenuItem("Saved");

            trayMenu = new ContextMenu(new MenuItem[]
                {
                    programList,
                    savedList,
                    new MenuItem("-"),
                    new MenuItem("Refresh", UpdateClick),
                    new MenuItem("Exit", ExitClick)
                }); ;

            trayIcon = new NotifyIcon()
            {
                Icon = Resources.icon,
                ContextMenu = trayMenu,
                Visible = true
            };

            Settings.LoadConfig();
            UpdateList();

            updateTask = Task.Run(async () =>
            {
                while (true)
                {
                    UpdateList();
                    await Task.Delay(10000);
                }
            });
        }

        private void ExitClick(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        public void UpdateClick(object sender, EventArgs e)
        {
            UpdateList();
        }

        public void UpdateList()
        {
            programList.MenuItems.Clear();
            programList.MenuItems.AddRange(utils.GetProcessItems());

            savedList.MenuItems.Clear();
            savedList.MenuItems.AddRange(utils.GetSavedProcessItems());
        }

    }
}
