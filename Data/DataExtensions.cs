using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data;

public static class DataExtensions
{
    public static async Task MigrateAsyncDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameShopContext>();
        await dbContext.Database.MigrateAsync();
    }

}