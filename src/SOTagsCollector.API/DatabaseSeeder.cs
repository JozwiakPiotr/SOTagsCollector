using Microsoft.EntityFrameworkCore;
using SOTagsCollector.API.Persistence;
using SOTagsCollector.API.Services;

namespace SOTagsCollector.API;

public static class DatabaseSeeder
{
    public static void SeedTags(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var sp = scope.ServiceProvider;
        var tagDb = sp.GetRequiredService<TagsDb>();
        var pendingMigrations = tagDb.Database.GetPendingMigrations();
        if(pendingMigrations.Any())
        {
            tagDb.Database.Migrate();
        }
        if (!tagDb.Tags.Any())
        {
            var tagService = sp.GetRequiredService<ITagService>();
            tagService.UpdateAll().GetAwaiter().GetResult();
        }
    }
}