using Denxorz.Satisfactory.Routes;

namespace SatisfactoryApp.Services;

public class RouteCalculationService
{
    public Task<SaveDetails> CalculateRoutesAsync(byte[] saveFileBytes)
    {
        using var stream = new MemoryStream(saveFileBytes);
        return Task.FromResult(SaveDetails.LoadFromStream(stream));
    }
}

