using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace PriorityManager
{
    public static class Settings
    {

        public static ConfigFile config;
        public static string configPath = AppDomain.CurrentDomain.BaseDirectory + @"config.json";

        public static void LoadConfig()
        {
            if (File.Exists(configPath))
            {
                string configText = File.ReadAllText(configPath);
                config = JsonConvert.DeserializeObject<ConfigFile>(configText);
            }
            else { InitConfig(); }
        }

        public static void SaveConfig()
        {
            if (File.Exists(configPath) && config != null)
            {
                StreamWriter sw = File.CreateText(configPath);
                JsonSerializer js = new JsonSerializer();
                js.Serialize(sw, config);
                sw.Close();
            } else { InitConfig(); }
        }

        public static void InitConfig()
        {
            config = new ConfigFile();
            config.Priority = 13;
            config.Processes = new List<string>();

            Console.WriteLine(configPath);
            StreamWriter sw = File.CreateText(configPath);
            JsonSerializer js = new JsonSerializer();
            js.Serialize(sw, config);
            sw.Close();
        }

    }

}
