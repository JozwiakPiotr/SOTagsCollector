using Microsoft.EntityFrameworkCore;
using SOTagsCollector.API.Persistence;

namespace SOTagsCollector.Integration.Tools;

public class TagsDbContextFactory
{
    private readonly string _connectionString;

    public TagsDbContextFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public TagsDb Create() => Create(_connectionString);
    
    public static TagsDb Create(string cs)
    {
        var options = new DbContextOptionsBuilder<TagsDb>()
            .UseSqlServer(cs);
        return new TagsDb(options.Options);
    }
}