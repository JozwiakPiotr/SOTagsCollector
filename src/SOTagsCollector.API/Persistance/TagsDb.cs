using Microsoft.EntityFrameworkCore;

namespace SOTagsCollector.API;

public class TagsDb : DbContext
{
    public TagsDb(DbContextOptions<TagsDb> options)
        : base(options) {}
    
    public DbSet<Tag> Tags { get; set; } = null!;
}
