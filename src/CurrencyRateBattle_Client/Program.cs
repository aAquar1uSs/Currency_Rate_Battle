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

builder.Services.AddSingleton<ICRBServerHttpClient, CRBServerHttpClient>();

builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession(new SessionOptions() {Cookie = new CookieBuilder() {Name = ".AspNetCore.Session.CBR"}});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Authorization}");

app.Run();
