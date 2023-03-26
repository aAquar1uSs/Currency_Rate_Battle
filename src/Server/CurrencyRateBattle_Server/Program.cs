using CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyHandlers.UpdateCurrencyRateHandlers;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GenerateRoomHandler;
using CurrencyRateBattleServer.ApplicationServices.HostedServices;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;
using CurrencyRateBattleServer.Dal;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddEndpointsApiExplorer();

services.ConfigureSwagger();

var host = builder.Host;

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();


host.ConfigureAppConfiguration(app =>
    {
        _ = app.AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();
    })
    .ConfigureLogging(loggerBuilder =>
    {
        _ = loggerBuilder.AddSerilog(logger);
        _ = loggerBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    })
    .ConfigureServices(service =>
    {
        service.ConfigureJWT(builder);

        _ = service.AddDbContext<CurrencyRateBattleContext>(option =>
            option.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));
        _ = service.AddDatabaseDeveloperPageExceptionFilter();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        _ = services.AddMediatR(typeof(GenerateRoomHandler), typeof(UpdateCurrencyRateHandler));
        service.ConfigureServices();

        //Disable automatic model state validation.
        _ = service.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.ConfigureClients(builder.Configuration);

        //ToDo Migrate to custom method
        _ = service.AddHostedService<CurrencyHostedService>()
            .AddHostedService<RoomHostedService>()
            .AddHostedService<RateHostedService>();

        _ = service.AddOptions()
            .AddScoped<IJwtManager, JwtManager>()
            .Configure<WebServerOptions>(builder.Configuration.GetSection(WebServerOptions.SectionName));

        _ = service.AddControllers();
    });

var app = builder.Build();

app.Services.InitDatabase();

if (app.Environment.IsDevelopment())
    _ = app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

await app.RunAsync();
