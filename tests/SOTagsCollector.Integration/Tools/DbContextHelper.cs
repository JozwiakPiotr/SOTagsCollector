using Microsoft.EntityFrameworkCore;

namespace SOTagsCollector.Integration.Tools;

public static class DbContextHelper
{
    public static async Task ExecuteAsync(this DbContext ctx, Func<DbContext, Task> func)
    {
        await func(ctx);
        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
    }
}