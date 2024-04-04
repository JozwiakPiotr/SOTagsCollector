using AutoFixture;
using SOTagsCollector.API.Entities;
using SOTagsCollector.API.Features.Tags.GetTags.Mapping;
using SOTagsCollector.Tools;

namespace SOTagsCollector.Unit.Features.Tags;

public class TagsMapperTests
{
    private readonly Fixture _fixture = FixtureFactory.Create();
    
    [Theory]
    [InlineData(100,50,50.0)]
    public void ToResponse_Always_CalculateAdditionalProperty(int givenPopulation, int count, double expected)
    {
        var tag = _fixture.Build<Tag>().With(t => t.Count, count).Create();

        var result = TagsMapper.ToResponse(new() { tag }, givenPopulation);
        
        Assert.Equal(expected, result[0].PopulationRatio);
    }
}