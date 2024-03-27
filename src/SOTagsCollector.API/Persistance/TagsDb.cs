using Microsoft.EntityFrameworkCore;
using SOTagsCollector.API.Entities;

namespace SOTagsCollector.API.Persistance;

public class TagsDb : DbContext
{
    public TagsDb(DbContextOptions<TagsDb> options)
        : base(options) {}
    
    public DbSet<Tag> Tags { get; set; } = null!;
}
