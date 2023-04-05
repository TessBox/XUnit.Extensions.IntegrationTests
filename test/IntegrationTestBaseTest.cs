using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace XUnit.Extensions.IntegrationTests.Test;

public class IntegrationTestBaseTest : IntegrationTest<Context>
{
    public IntegrationTestBaseTest(ITestOutputHelper testOutputHelper, Context context)
        : base(testOutputHelper, context) { }

    [Fact]
    public void Test_Ioc_true()
    {
        // act
        var service = Context.GetService<ITestService>();

        // assert
        Assert.NotNull(service);
        Assert.True(service.True);
    }
}

public class Context : TestContext
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
    {
        services.AddTransient<ITestService, TestService>();
    }

    protected override IEnumerable<string> GetSettingsFiles()
    {
        return new[] { "appsettings.json" };
    }
}

public interface ITestService
{
    bool True { get; }
}

public class TestService : ITestService
{
    public bool True => true;
}
