using System;
using System.Collections.Generic;
using IrisXMPPServer.Database;

namespace IrisXMPPServer.Settings
{
    public static class SettingsParser
    {
        public static ServerSettings Parse(IEnumerable<string> lines)
        {
            ServerSettings settings = new ServerSettings();

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }

                int separator = line.IndexOf('=');
                if (separator < 0)
                {
                    continue;
                }

                string key = line.Substring(0, separator).Trim().ToUpperInvariant();
                string value = line.Substring(separator + 1).Trim().Replace("\"", "");
                Apply(settings, key, value);
            }

            return settings;
        }

        private static void Apply(ServerSettings settings, string key, string value)
        {
            switch (key)
            {
                case "HOSTNAME":
                    settings.HostName = value;
                    break;
                case "SERVER_PORT":
                    settings.Port = ParseNumber(key, value);
                    break;
                case "DATABASE_ENGINE":
                    settings.DatabaseEngine = value.Equals("mysql", StringComparison.OrdinalIgnoreCase)
                        ? SupportedEngine.MYSQL
                        : SupportedEngine.UNKNOWN;
                    break;
                case "DATABASE_HOST":
                    settings.DatabaseHost = value;
                    break;
                case "DATABASE_PORT":
                    settings.DatabasePort = ParseNumber(key, value);
                    break;
                case "DATABASE_USERNAME":
                    settings.DatabaseUser = value;
                    break;
                case "DATABASE_PASSWORD":
                    settings.DatabasePassword = value;
                    break;
                case "DATABASE_SCHEMA":
                    settings.DatabaseSchema = value;
                    break;
            }
        }

        private static int ParseNumber(string key, string value)
        {
            if (!int.TryParse(value, out int number))
            {
                throw new FormatException($"Invalid value for {key}: '{value}' is not a number.");
            }

            return number;
        }
    }
}
