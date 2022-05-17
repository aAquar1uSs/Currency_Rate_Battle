using System.Text;
using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Managers;
using CurrencyRateBattleServer.Managers.Interfaces;
using CurrencyRateBattleServer.Services;
using CurrencyRateBattleServer.Services.Interfaces;
using CurrencyRateBattleServer.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo {Title = "MyAPI", Version = "v1"});
    opt.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
            },
            Array.Empty<string>()
        }
    });
});

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
        _ = service.AddDatabaseDeveloperPageExceptionFilter();
        //Disable automatic model state validation.
        _ = service.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        _ = service.AddOptions()
            .AddSingleton<IJwtManager, JwtManager>()
            .AddSingleton<IEncoder, Sha256Encoder>()
            .AddSingleton<IAccountService, AccountService>();
        _ = service.AddControllers();

        _= service.AddScoped<CurrencyRateBattleContext>();
    });

var app = builder.Build();

InitDatabase(app.Services);

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


static void InitDatabase(IServiceProvider serviceProvider)
{
    using var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetService<CurrencyRateBattleContext>();
    if (context is not null)
        context.Database.Migrate();
}
