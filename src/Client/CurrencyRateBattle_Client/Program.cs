using CRBClient.Helpers;
using CRBClient.Services;
using CRBClient.Services.Interfaces;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

//Configure logger
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(logger)
    .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

builder.Services.AddControllersWithViews();
builder.Services.Configure<WebServerOptions>(
    builder.Configuration.GetSection(WebServerOptions.SectionName));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICRBServerHttpClient, CRBServerHttpClient>();

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRateService, UserRateService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    _ = app.UseHsts();
}

app.UseSession(new SessionOptions() { Cookie = new CookieBuilder() { Name = ".AspNetCore.Session.CBR" } });
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Authorization}");

await app.RunAsync();
