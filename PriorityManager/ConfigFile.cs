using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityManager
{
    public class ConfigFile
    {

        public ProcessPriorityClass PriorityClass { get; set; }

        public bool GuiProcessesOnly { get; set; }

        public List<string> Processes { get; set; }

    }
}
