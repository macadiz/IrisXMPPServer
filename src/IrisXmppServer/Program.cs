using IrisXMPPServer.Threads;
using System;
using System.Linq;

namespace IrisXMPPServer
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
            string[] lines = System.IO.File.ReadAllLines(@"IrisXmppServer.conf");

            foreach (string line in lines)
            {
                if (line.FirstOrDefault() != '#')
                {
                    string[] keyValue = line.Split('=');
                    if (keyValue.Length == 2)
                    {
                        string value = keyValue[1].Trim().Replace("\"", "");
                        if (line.ToUpper().Contains("HOSTNAME"))
                        {
                            CoreClasses.Configuration.hostName = value;
                        }
                        if (line.ToUpper().Contains("SERVER_PORT"))
                        {
                            CoreClasses.Configuration.port = Int32.Parse(value);
                        }
                        if (line.ToUpper().Contains("DATABASE_ENGINE"))
                        {
                            Database.SupportedEngine engine;
                            if (value.ToLower().Equals("mysql"))
                            {
                                engine = Database.SupportedEngine.MYSQL;
                            }
                            else
                            {
                                engine = Database.SupportedEngine.UNKNOWN;
                            }
                            CoreClasses.Configuration.databaseEngine = engine;
                        }
                        if (line.ToUpper().Contains("DATABASE_HOST"))
                        {
                            CoreClasses.Configuration.databaseHost = value;
                        }
                        if (line.ToUpper().Contains("DATABASE_PORT"))
                        {
                            CoreClasses.Configuration.databasePort = Int32.Parse(value);
                        }
                        if (line.ToUpper().Contains("DATABASE_USERNAME"))
                        {
                            CoreClasses.Configuration.databaseUser = value;
                        }
                        if (line.ToUpper().Contains("DATABASE_PASSWORD"))
                        {
                            CoreClasses.Configuration.databasePassword = value;
                        }
                        if (line.ToUpper().Contains("DATABASE_SCHEMA"))
                        {
                            CoreClasses.Configuration.databaseSchema = value;
                        }
                    }
                }
            }
        }
    }
}
