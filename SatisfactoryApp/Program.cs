using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudExtensions.Services;
using SatisfactoryApp;
using SatisfactoryApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();

// Register custom services
builder.Services.AddScoped<RouteCalculationService>();
builder.Services.AddScoped<FactoryStore>();
builder.Services.AddScoped<StationStore>();

await builder.Build().RunAsync();
