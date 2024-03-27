using AutoFixture;
using AutoFixture.AutoMoq;

namespace SOTagsCollector.Unit;

public static class FixtureFactory
{
    public static Fixture Create()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization());
        return fixture;
    }
}