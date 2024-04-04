using AutoFixture.Xunit2;
using SOTagsCollector.Tools;

namespace SOTagsCollector.Unit.Tools;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(FixtureFactory.Create)
    {}
}