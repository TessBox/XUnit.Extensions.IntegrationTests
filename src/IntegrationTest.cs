using Xunit;
using Xunit.Abstractions;

namespace XUnit.Extensions.IntegrationTests;

public class IntegrationTest<TContext> : OrderTestsBase, IClassFixture<TContext>
    where TContext : TestContext
{
    public IntegrationTest(ITestOutputHelper testOutputHelper, TContext context)
    {
        Context = context;
        context.AttachITestOutputHelper(testOutputHelper);
    }

    protected TContext Context { get; }
}
