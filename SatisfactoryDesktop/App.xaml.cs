using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Velopack;

namespace SatisfactoryDesktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [STAThread]
    private static void Main(string[] args)
    {
        VelopackApp.Build()
            .SetAutoApplyOnStartup(true)
            .Run();

        var updateUrl = GetUpdateUrl();
        if (!string.IsNullOrWhiteSpace(updateUrl))
        {
            try
            {
                UpdateOnStartupAsync(updateUrl).GetAwaiter().GetResult();
            }
            catch
            {
                // Ignore update failures and continue startup.
            }
        }

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }

    private static async Task UpdateOnStartupAsync(string updateUrl)
    {
        var updateManager = new UpdateManager(updateUrl);
        var updateInfo = await updateManager.CheckForUpdatesAsync();
        if (updateInfo == null)
        {
            return;
        }

        await updateManager.DownloadUpdatesAsync(updateInfo);
        updateManager.ApplyUpdatesAndRestart(updateInfo);
    }

    private static string? GetUpdateUrl()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
            .Build();

        return configuration["UpdateUrl"];
    }
}

