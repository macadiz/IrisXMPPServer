using IrisXMPPServer.Database;

namespace IrisXMPPServer.Settings
{
    public class ServerSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; } = 5222;

        public SupportedEngine DatabaseEngine { get; set; } = SupportedEngine.UNKNOWN;
        public string DatabaseHost { get; set; }
        public int DatabasePort { get; set; }
        public string DatabaseUser { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseSchema { get; set; }
    }
}
