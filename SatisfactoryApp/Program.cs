using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudExtensions.Services;
using SatisfactoryApp;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Services.Resources;
using SatisfactoryApp.Services.Stations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();

// Register custom services
builder.Services.AddScoped<FactoryStore>();
builder.Services.AddScoped<StationStore>();
builder.Services.AddScoped<ResourceStore>();

// Configure Sentry
var sentryDsn = builder.Configuration["Sentry:Dsn"] ?? "";
if (!string.IsNullOrEmpty(sentryDsn))
{
    builder.UseSentry(options =>
    {
        options.Dsn = sentryDsn;
        options.Debug = builder.HostEnvironment.IsDevelopment();
        options.TracesSampleRate = builder.HostEnvironment.IsDevelopment() ? 1.0 : 0.1;
        options.Environment = builder.HostEnvironment.Environment;
        options.Release = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    });
}

await builder.Build().RunAsync();
