using IrisXMPPServer.Database;
using IrisXMPPServer.Settings;

namespace IrisXMPPServer.Tests.Settings;

public class SettingsParserTests
{
    [Fact]
    public void Parse_ReadsEveryKnownKey()
    {
        string[] lines =
        [
            "HOSTNAME = \"iris.local\"",
            "SERVER_PORT = 5333",
            "DATABASE_ENGINE = \"mysql\"",
            "DATABASE_HOST = \"db.local\"",
            "DATABASE_PORT = 3307",
            "DATABASE_USERNAME = \"iris\"",
            "DATABASE_PASSWORD = \"secret\"",
            "DATABASE_SCHEMA = \"iris_db\"",
        ];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal("iris.local", settings.HostName);
        Assert.Equal(5333, settings.Port);
        Assert.Equal(SupportedEngine.MYSQL, settings.DatabaseEngine);
        Assert.Equal("db.local", settings.DatabaseHost);
        Assert.Equal(3307, settings.DatabasePort);
        Assert.Equal("iris", settings.DatabaseUser);
        Assert.Equal("secret", settings.DatabasePassword);
        Assert.Equal("iris_db", settings.DatabaseSchema);
    }

    [Fact]
    public void Parse_IgnoresCommentLinesAndBlankLines()
    {
        string[] lines =
        [
            "# HOSTNAME = \"commented.out\"",
            "",
            "HOSTNAME = \"iris.local\"",
        ];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal("iris.local", settings.HostName);
    }

    [Fact]
    public void Parse_MatchesKeysCaseInsensitively()
    {
        string[] lines = ["hostname = \"iris.local\"", "Server_Port = 5222"];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal("iris.local", settings.HostName);
        Assert.Equal(5222, settings.Port);
    }

    [Fact]
    public void Parse_MatchesKeysExactly_NotBySubstring()
    {
        string[] lines = ["MY_HOSTNAME_BACKUP = \"wrong\"", "HOSTNAME = \"iris.local\""];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal("iris.local", settings.HostName);
    }

    [Fact]
    public void Parse_KeepsEqualSignsInsideValues()
    {
        string[] lines = ["DATABASE_PASSWORD = \"pa=ss=word\""];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal("pa=ss=word", settings.DatabasePassword);
    }

    [Fact]
    public void Parse_TrimsWhitespaceAroundKeysAndValues()
    {
        string[] lines = ["   HOSTNAME   =    \"iris.local\"   "];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal("iris.local", settings.HostName);
    }

    [Fact]
    public void Parse_UnknownDatabaseEngine_MapsToUnknown()
    {
        string[] lines = ["DATABASE_ENGINE = \"postgres\""];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal(SupportedEngine.UNKNOWN, settings.DatabaseEngine);
    }

    [Fact]
    public void Parse_MissingPort_DefaultsTo5222()
    {
        ServerSettings settings = SettingsParser.Parse(["HOSTNAME = \"iris.local\""]);

        Assert.Equal(5222, settings.Port);
    }

    [Fact]
    public void Parse_UnknownKeys_AreIgnored()
    {
        string[] lines = ["SOMETHING_ELSE = \"value\"", "HOSTNAME = \"iris.local\""];

        ServerSettings settings = SettingsParser.Parse(lines);

        Assert.Equal("iris.local", settings.HostName);
    }

    [Fact]
    public void Parse_NonNumericPort_ThrowsFormatExceptionNamingTheKey()
    {
        string[] lines = ["SERVER_PORT = \"abc\""];

        FormatException error = Assert.Throws<FormatException>(() => SettingsParser.Parse(lines));

        Assert.Contains("SERVER_PORT", error.Message);
    }
}
