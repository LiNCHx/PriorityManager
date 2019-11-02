using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriorityManager
{
    class Utils
    {

        public int Priority = 13; /* NORMAL: 8, HIGHER: 10, HIGH: 13 */

        public Utils() { }

        public MenuItem GetProcessItem(string pName)
        {
            MenuItem item = new MenuItem()
            {
                Text = pName
            };

            item.Click += ProcessRemoveEvent;

            return item;
        }

        public MenuItem GetProcessItem(Process pProcess)
        {
            MenuItem item = new MenuItem()
            {
                Text = pProcess.ProcessName,
                Tag = pProcess,
                Checked = IsPriotized(pProcess)
            };

            item.Click += ProcessToggleEvent;

            return item;
        }

        public MenuItem[] GetSavedProcessItems()
        {
            List<MenuItem> mList = new List<MenuItem>();
            List<string> nList = Settings.config.Processes;

            foreach (string n in nList)
            {
                mList.Add(GetProcessItem(n));
            }

            return mList.ToArray();
        }

        public MenuItem[] GetProcessItems()
        {
            List<MenuItem> mList = new List<MenuItem>();
            Process[] pList = GetProcesses();

            foreach (Process p in pList)
            {
                mList.Add(GetProcessItem(p));
            }

            return mList.ToArray();
        }

        public Process[] GetProcesses()
        {
            List<Process> pList = new List<Process>();
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    pList.Add(p);
                }
            }

            return pList.ToArray();
        }

        public bool IsPriotized(Process pProcess)
        {
            if (pProcess.BasePriority == Priority)
            {
                return true;
            }

            if (Settings.config.Processes.Contains(pProcess.ProcessName))
            {
                pProcess.PriorityClass = ProcessPriorityClass.High;
                return true;
            }

            return false;
        }

        public void ProcessRemoveEvent(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string name = item.Text;

            Settings.config.Processes.RemoveAll(pn => pn == name);
            Settings.SaveConfig();

            item.Parent.MenuItems.Remove(item);
        }

        public void ProcessToggleEvent(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            Process proc = item.Tag as Process;

            item.Checked = !item.Checked;
            if (item.Checked)
            {
                proc.PriorityClass = ProcessPriorityClass.High;
                Settings.config.Processes.Add(proc.ProcessName);
            } else
            {
                proc.PriorityClass = ProcessPriorityClass.Normal;
                Settings.config.Processes.RemoveAll(pn => pn == proc.ProcessName);
            }

            Settings.SaveConfig();
        }

    }
}
