using IrisXMPPServer.Settings;
using IrisXMPPServer.Threads;

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
            ServerSettings settings = SettingsParser.Parse(System.IO.File.ReadAllLines(@"IrisXmppServer.conf"));

            CoreClasses.Configuration.hostName = settings.HostName;
            CoreClasses.Configuration.port = settings.Port;
            CoreClasses.Configuration.databaseEngine = settings.DatabaseEngine;
            CoreClasses.Configuration.databaseHost = settings.DatabaseHost;
            CoreClasses.Configuration.databasePort = settings.DatabasePort;
            CoreClasses.Configuration.databaseUser = settings.DatabaseUser;
            CoreClasses.Configuration.databasePassword = settings.DatabasePassword;
            CoreClasses.Configuration.databaseSchema = settings.DatabaseSchema;
        }
    }
}
