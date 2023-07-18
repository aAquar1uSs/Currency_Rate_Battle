using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManager.Dal.infrastructure;

public static class DbConfiguration
{
    public static void InitDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AccountManagerDbContext>();
        if (context is not null)
            context.Database.Migrate();
    }

    public static void ConfigureServices(this IServiceCollection service)
    {
    }
}
