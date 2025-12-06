using Microsoft.JSInterop;

namespace SatisfactoryApp.Utils;

public static class DebugLogger
{
    public static async Task LogAsync(IJSRuntime jsRuntime, string message)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("console.log", $"[SatisfactoryApp] {message}");
        }
        catch
        {
            // Ignore if console is not available
        }
    }

    public static async Task LogErrorAsync(IJSRuntime jsRuntime, string message, Exception? ex = null)
    {
        try
        {
            var errorMessage = ex != null ? $"{message}: {ex.Message}" : message;
            await jsRuntime.InvokeVoidAsync("console.error", $"[SatisfactoryApp] ERROR: {errorMessage}");
            if (ex != null)
            {
                await jsRuntime.InvokeVoidAsync("console.error", $"[SatisfactoryApp] Stack: {ex.StackTrace}");
            }
        }
        catch
        {
            // Ignore if console is not available
        }
    }

    public static async Task LogObjectAsync(IJSRuntime jsRuntime, string label, object obj)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("console.log", $"[SatisfactoryApp] {label}:", obj);
        }
        catch
        {
            // Ignore if console is not available
        }
    }
}






