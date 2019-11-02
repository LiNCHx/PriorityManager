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

        private Utils utils;
        private Task updateTask;

        public PriorityManager()
        {
            Settings.LoadConfig();

            utils = new Utils();

            programList = new MenuItem("Programs");
            savedList = new MenuItem("Saved");

            // Init Tray Menu
            trayMenu = new ContextMenu(new MenuItem[]
                {
                    programList,
                    savedList,
                    new MenuItem("-"),
                    new MenuItem("Refresh", UpdateClick),
                    new MenuItem("About", AboutClick),
                    new MenuItem("-"),
                    new MenuItem("Exit", ExitClick)
                }); ;

            // Init Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.icon,
                ContextMenu = trayMenu,
                Visible = true
            };

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

        public void AboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("PriorityManager by LiNCHx\nIcon made by Freepik from www.flaticon.com", "About");
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
