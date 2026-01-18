using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows;
using Denxorz.Satisfactory.Routes;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace SatisfactoryDesktop;

public partial class MainWindow : Window
{
    private const string IconPackUri = "pack://application:,,,/Assets/icons8-graph-48.png";
    private const string DropZoneHoverBrush = "#E59345";
    private const string DropZoneNormalBrush = "#555555";
    private string? _saveFilePath;

    public MainWindow()
    {
        InitializeComponent();
        Title = $"Statisfactory Loader v{GetAppVersion()}";
    }

    private static string GetAppVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyVersion = assembly.GetName().Version?.ToString();
        return string.IsNullOrWhiteSpace(assemblyVersion)
            ? "dev"
            : assemblyVersion.TrimStart('v', 'V');
    }

    private async Task StartProcessingAsync()
    {
        var siteUrl = GetSiteUrl();

        if (!Uri.TryCreate(siteUrl, UriKind.Absolute, out var siteUri) ||
            (siteUri.Scheme != Uri.UriSchemeHttp && siteUri.Scheme != Uri.UriSchemeHttps))
        {
            ShowError("Invalid SiteUrl in appsettings.");
            return;
        }

        SetProcessing(true);
        try
        {
            var json = await BuildJsonAsync();

            var wrapperPath = await WriteWrapperHtmlAsync(siteUri, _saveFilePath, json);

            Process.Start(new ProcessStartInfo
            {
                FileName = wrapperPath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
        finally
        {
            SetProcessing(false);
        }
    }

    private async Task<string> BuildJsonAsync()
    {
        using var stream = File.OpenRead(_saveFilePath);
        SaveDetails? result = null;
        await Task.Run(() => result = SaveDetails.LoadFromStream(stream));

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(result, options);
    }

    private static async Task<string> WriteWrapperHtmlAsync(Uri siteUri, string savePath, string json)
    {
        var filename = Path.GetFileName(savePath);
        var escapedJson = EscapeJsonForScript(json);
        var iconDataUri = GetIconDataUri();

        var tempDir = Path.Combine(Path.GetTempPath(), "statisfactory-pre-loader");
        Directory.CreateDirectory(tempDir);
        Directory.GetFiles(tempDir).ToList().ForEach(File.Delete);
        var outputPath = Path.Combine(tempDir, $"statisfactory-{DateTime.Now:yyyyMMdd-HHmmss}.html");

        var html = BuildWrapperHtml(siteUri, escapedJson, filename, iconDataUri);
        await File.WriteAllTextAsync(outputPath, html, new UTF8Encoding(false));

        return outputPath;
    }

    private static string GetIconDataUri()
    {
        var resourceInfo = Application.GetResourceStream(new Uri(IconPackUri, UriKind.Absolute))
            ?? throw new InvalidOperationException("Icon resource not found.");

        using var memory = new MemoryStream();
        resourceInfo.Stream.CopyTo(memory);
        var base64 = Convert.ToBase64String(memory.ToArray());
        return $"data:image/png;base64,{base64}";
    }

    private static string EscapeJsonForScript(string json)
    {
        return json.Replace("</script>", "<\\/script>", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildWrapperHtml(Uri siteUrl, string json, string filename, string iconDataUri)
    {
        var encodedUrl = WebUtility.HtmlEncode(siteUrl.ToString());
        var filenameLiteral = JsonSerializer.Serialize(filename);
        var originLiteral = JsonSerializer.Serialize(siteUrl.ToString());
        var iconLine = $"  <link rel=\"icon\" type=\"image/png\" href=\"{iconDataUri}\" />\n";

        return $$"""
            <!doctype html>
            <html>
            <head>
              <meta charset="utf-8" />
              <title>Statisfactory</title>
            {{iconLine}}
            </head>
            <body style="margin:0">
              <iframe id="app" src="{{encodedUrl}}" style="border:0;width:100vw;height:100vh;"></iframe>

              <script id="statisfactory-data" type="application/json">
            {{json}}
              </script>

              <script>
                document.addEventListener('DOMContentLoaded', () => {
                  const dataElement = document.getElementById('statisfactory-data');
                  if (!dataElement) return;
                  const json = dataElement.textContent;
                  const filename = {{filenameLiteral}};
                  const targetOrigin = {{originLiteral}};
                  const iframe = document.getElementById('app');
                  if (!iframe) return;
                  const sendPayload = () => {
                    if (!iframe.contentWindow) return;
                    iframe.contentWindow.postMessage({
                      type: 'statisfactory-json',
                      payload: { json, filename }
                    }, targetOrigin);
                  };
                  window.addEventListener('message', (event) => {
                    if (event.source !== iframe.contentWindow) return;
                    if (event.data?.type === 'statisfactory-ready') {
                      sendPayload();
                    }
                  });
                  iframe.addEventListener('load', () => {
                    sendPayload();
                    let attempts = 0;
                    const retry = setInterval(() => {
                      if (attempts >= 10) {
                        clearInterval(retry);
                        return;
                      }
                      sendPayload();
                      attempts += 1;
                    }, 500);
                  });
                });
              </script>
            </body>
            </html>
            """;
    }

    private void SetProcessing(bool isProcessing)
    {
        DropZone.IsEnabled = !isProcessing;
        Spinner.Visibility = isProcessing ? Visibility.Visible : Visibility.Collapsed;
    }

    private void ShowError(string message)
    {
        MessageBox.Show(this, message, "Statisfactory Loader", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    private static string GetSiteUrl()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
            .Build();

        return config["SiteUrl"]?.Trim() ?? "";
    }

    private async void DropZone_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        await SelectSaveFileAsync();
    }

    private void DropZone_DragEnter(object sender, DragEventArgs e)
    {
        if (HasSingleSaveFile(e))
        {
            e.Effects = DragDropEffects.Copy;
            DropZoneOutline.Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(DropZoneHoverBrush));
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    private void DropZone_DragLeave(object sender, DragEventArgs e)
    {
        DropZoneOutline.Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(DropZoneNormalBrush));
        e.Handled = true;
    }

    private async void DropZone_Drop(object sender, DragEventArgs e)
    {
        DropZoneOutline.Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(DropZoneNormalBrush));
        if (!HasSingleSaveFile(e))
        {
            ShowError("Drop a single .sav file.");
            return;
        }

        if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length == 1)
        {
            _saveFilePath = files[0];
            await StartProcessingAsync();
        }
        e.Handled = true;
    }

    private async Task SelectSaveFileAsync()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select a Satisfactory save file",
            Filter = "Satisfactory save (*.sav)|*.sav|All files (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialog.ShowDialog(this) == true)
        {
            _saveFilePath = dialog.FileName;
            await StartProcessingAsync();
        }
    }

    private static bool HasSingleSaveFile(DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return false;
        }

        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files || files.Length != 1)
        {
            return false;
        }

        return string.Equals(Path.GetExtension(files[0]), ".sav", StringComparison.OrdinalIgnoreCase);
    }
}