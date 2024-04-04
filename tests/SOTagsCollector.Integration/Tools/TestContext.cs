namespace SOTagsCollector.Integration.Tools;

public static class TestContext
{
    public static string TestDataDirectory => Path.Combine(AppContext.BaseDirectory, @"TestData\StackExchangeApi");
}