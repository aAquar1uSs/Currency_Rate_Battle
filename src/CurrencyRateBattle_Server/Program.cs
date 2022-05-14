using CurrencyRateBattle_Server.Contexts;
using CurrencyRateBattle_Server.Services;
using CurrencyRateBattle_Server.Services.Impl;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var host = builder.Host;

host.ConfigureAppConfiguration(app =>
    {
        _ = app.AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();
    })
    .ConfigureLogging(loggerBuilder =>
    {
        _ = loggerBuilder.ClearProviders();
        _ = loggerBuilder.AddSerilog(new LoggerConfiguration()
            .WriteTo.File("AppLog.log")
            .CreateLogger());
    })
    .ConfigureServices(service =>
    {
        _ = service.AddDbContext<CurrencyRateBattleContext>();
        _ = service.AddOptions()
            .AddSingleton<IAccountService, AccountService>();
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
    _ = app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

await app.RunAsync();
