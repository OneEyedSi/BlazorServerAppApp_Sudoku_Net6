using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SudokuClassLibrary;
using SudokuWebApp.Data;
using SudokuWebApp.Shared.Classes;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;
using SudokuDataAccess;
using Microsoft.EntityFrameworkCore;

// This configures the app to read configuration from, amongst other sources,
// appsettings.{environment}.json  and appsettings.json.
// See MS Learn article "Configuration in ASP.NET Core", 
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0#default-host-configuration-sources
var builder = WebApplication.CreateBuilder(args);

// Configure SeriLog.  However, this does not replace the built-in ASP.NET Core logger.
Log.Logger = new LoggerConfiguration()
    // NB: To read from appsettings.json requires package Serilog.Settings.Configuration.
    // To use an ExpressionTemplate in appsettings.json (and probably if hard-coded in 
    // WriteTo.Console(...) as well, not sure), requires package Serilog.Expressions.
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<GameState>();
builder.Services.AddDbContextFactory<DataContext>(options => 
    options.UseSqlite("Data Source=Data/Games.db"));

// Replace the built-in ASP.NET Core logger.
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Allows us to control ASP.NET request logging, and particularly to dial it down because it's
// very chatty.
app.UseSerilogRequestLogging();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Log.Information("Application about to run...");

app.Run();
