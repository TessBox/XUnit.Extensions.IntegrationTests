namespace XUnit.Extensions.IntegrationTests.Test;

public class OrderTestsBaseTest : OrderTestsBase
{
    private static int index = 10;

    [Fact, TestPriority(10)]
    public void Test10()
    {
        Assert.Equal(10, index);
        index = 20;
    }

    [Fact, TestPriority(30)]
    public void Test30()
    {
        Assert.Equal(30, index);
        index = 40;
    }

    [Fact, TestPriority(20)]
    public void Test20()
    {
        Assert.Equal(20, index);
        index = 30;
    }

    [Fact, TestPriority(40)]
    public void ATest40()
    {
        Assert.Equal(40, index);
        index = 50;
    }
}
