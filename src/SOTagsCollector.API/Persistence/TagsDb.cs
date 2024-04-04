using Microsoft.EntityFrameworkCore;
using SOTagsCollector.API.Entities;

namespace SOTagsCollector.API.Persistence;

public class TagsDb : DbContext
{
    public TagsDb(DbContextOptions<TagsDb> options)
        : base(options) {}
    
    public virtual DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TagsDb).Assembly);
    }
}
