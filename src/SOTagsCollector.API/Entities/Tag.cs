namespace SOTagsCollector.API.Entities;

public class Tag
{
    public Tag(string name, int count)
    {
        Name = name;
        Count = count;
    }
    public string Name { get; set; }
    public int Count { get; set; }
}
