using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SatisfactoryApp;
using SatisfactoryApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

// Register custom services
builder.Services.AddScoped<RouteCalculationService>();
builder.Services.AddSingleton<FactoryStore>();
builder.Services.AddSingleton<StationStore>();

await builder.Build().RunAsync();
