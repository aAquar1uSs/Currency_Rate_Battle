using System.Text;
using CurrencyRateBattle_Server.Contexts;
using CurrencyRateBattle_Server.Managers;
using CurrencyRateBattle_Server.Managers.Impl;
using CurrencyRateBattle_Server.Services;
using CurrencyRateBattle_Server.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        _ = service.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
            option.SaveToken = true;
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        _ = service.AddDbContext<CurrencyRateBattleContext>(option =>
            option.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDb")));

        _ = service.AddOptions()
            .AddSingleton<IJwtManager, JwtManger>()
            .AddSingleton<IAccountService, AccountService>();
        _ = service.AddControllers();
    });

var app = builder.Build();

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
