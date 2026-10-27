using IrisXMPPServer.CoreClasses;

namespace IrisXMPPServer.Tests.CoreClasses;

public class ConfigurationTests
{
    [Fact]
    public void Properties_KeepTheValuesAssignedToThem()
    {
        Configuration.hostName = "iris.local";
        Configuration.port = 5222;

        Assert.Equal("iris.local", Configuration.hostName);
        Assert.Equal(5222, Configuration.port);
    }
}
