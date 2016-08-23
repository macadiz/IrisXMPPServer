using NetCoreXmppServer.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreXmppServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoadConfiguration();
            MainConnectionThread mainThread = new MainConnectionThread();
            mainThread.RunThread();
        }

        public static void LoadConfiguration()
        {
            string[] lines = System.IO.File.ReadAllLines(@"dotNetXmppServer.conf");

            foreach (string line in lines)
            {
                if (line.FirstOrDefault() != '#')
                {
                    string[] keyValue = line.Split('=');
                    if (line.ToUpper().Contains("HOSTNAME"))
                    {
                        CoreClasses.Configuration.hostName = keyValue[1].Trim().Replace("\"", "");
                    }
                    if (line.ToUpper().Contains("PORT"))
                    {
                        CoreClasses.Configuration.port = Int32.Parse(keyValue[1].Trim().Replace("\"", ""));
                    }
                }
            }
        }
    }
}
