namespace SOTagsCollector.API;

public class Tag
{
    public Tag(string name, int count, double populationRatio)
    {
        Name = name;
        Count = count;
        PopulationRatio = populationRatio;
    }
    public string Name { get; set; }
    public int Count { get; set; }
    public double PopulationRatio { get; set; }
}
