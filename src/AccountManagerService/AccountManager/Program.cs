using AccountManager.ApplicationServices.Infrastructure.JWTManager;
using AccountManager.Dal;
using AccountManager.Dal.infrastructure;
using AccountManager.Infrastructure;
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

        _ = service.AddDbContext<AccountManagerDbContext>(option =>
            option.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        service.ConfigureServices();

        //Disable automatic model state validation.
        _ = service.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        _ = service.AddOptions()
            .AddScoped<IJwtManager, JwtManager>();

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