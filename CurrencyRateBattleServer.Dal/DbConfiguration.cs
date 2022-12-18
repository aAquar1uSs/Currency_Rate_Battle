using CurrencyRateBattleServer.Dal.Repositories;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Dal.Services;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRateBattleServer.Dal;

public static class DbConfiguration
{
    public static void InitDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetService<CurrencyRateBattleContext>();
        if (context is not null)
            context.Database.Migrate();
    }

    public static void ConfigureServices(this IServiceCollection service)
    {
        _ = service.AddScoped<IAccountRepository, AccountRepository>()
                 .AddScoped<IRoomRepository, RoomRepository>()
                 .AddScoped<IRateRepository, RateRepository>()
                 .AddScoped<ICurrencyStateRepository, CurrencyStateRepository>()
                 .AddScoped<IAccountHistoryRepository, AccountHistoryRepository>()
                 .AddScoped<IRateCalculationRepository, RateCalculationRepository>()
                 .AddScoped<IPaymentRepository, PaymentRepository>()
                 .AddScoped<IUserRatingQueryRepository, UserRatingQueryRepository>()
                 .AddScoped<IUserRepository, UserRepository>()
                 .AddScoped<IRoomQueryRepository, RoomQueryRepository>()
                 .AddScoped<ICurrencyRepository, CurrencyRepository>();
         }
}
