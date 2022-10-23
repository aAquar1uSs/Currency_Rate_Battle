using CurrencyRateBattleServer.Dal.Services;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Services;
using CurrencyRateBattleServer.Services.HostedServices;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRateBattleServer.Dal;

public static class PostgresConfiguration
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
        _ = service.AddSingleton<IAccountService, AccountService>()
            .AddScoped<IRoomService, RoomService>()
            .AddScoped<IRateService, RateService>()
            .AddScoped<ICurrencyStateService, CurrencyStateService>()
            .AddScoped<IAccountHistoryService, AccountHistoryService>()
            .AddScoped<IRateCalculationService, RateCalculationService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IRatingService, RatingService>();
    }
}
