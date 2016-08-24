namespace IrisXMPPServer.CoreClasses
{
    public static class Configuration
    {
        public static string hostName { get; set; }
        public static int port { get; set; }

        public static Database.SupportedEngine databaseEngine { get; set; }
        public static string databaseHost { get; set; }
        public static int databasePort { get; set; }
        public static string databaseUser { get; set; }
        public static string databasePassword { get; set; }
        public static string databaseSchema { get; set; }
    }
}
