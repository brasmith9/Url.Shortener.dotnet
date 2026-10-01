using Microsoft.EntityFrameworkCore;
using Url.Shortner.Entity;

namespace Url.Shortner.Extensions;

public static class DatabaseExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        if (context is null)
        {
            throw new InvalidOperationException("The database context is null.");
        }
        
        var pendingMigrations = context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().ToList();

        if (pendingMigrations.Any())
        {
            var count = pendingMigrations.Count;
            logger.LogInformation("Applying {Count} migration ",  count);
            context.Database.MigrateAsync().Wait();
            
            logger.LogInformation("{Count} migrations applied successsfuly ",  count);
            return;
        }
        
        logger.LogInformation("No migrations applied");
    }
}