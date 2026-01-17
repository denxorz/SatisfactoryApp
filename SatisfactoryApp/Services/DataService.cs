using System.Net.Http.Json;
using System.Text.Json;
using Denxorz.Satisfactory.Routes;
using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Services.Resources;
using SatisfactoryApp.Services.Stations;

namespace SatisfactoryApp.Services;

public class DataService
{
    private const string TestDataFileName = "testdata.json";

    public static async Task<SaveData> GetTestDataAsync(HttpClient httpClient)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return await httpClient.GetFromJsonAsync<SaveData>(TestDataFileName, options)
            ?? throw new InvalidOperationException($"Failed to load data from {TestDataFileName}");
    }

    public static string ExportToJson(FactoryStore factoryStore, StationStore stationStore, ResourceStore resourceStore)
    {
        var testData = new SaveData
        {
            PowerCircuits = [.. factoryStore.PowerCircuits],
            Factories = [.. factoryStore.Factories],
            Stations = [.. stationStore.Stations],
            Uploaders = [.. stationStore.Uploaders],
            Resources = [.. resourceStore.Resources],
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(testData, options);
    }

    public static SaveData ImportFromJson(string jsonContent, string? filename)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<SaveData>(jsonContent, options)
            ?? throw new InvalidOperationException("Failed to load data from JSON");
        data.Filename = filename;

        return data;
    }
}

public class SaveData
{
    public string? Filename { get; set; }
    public List<PowerCircuit> PowerCircuits { get; set; } = [];
    public List<Factory> Factories { get; set; } = [];
    public List<Station> Stations { get; set; } = [];
    public List<Uploader> Uploaders { get; set; } = [];
    public List<Resource> Resources { get; set; } = [];

    public static SaveData FromSaveDetails(string? filename, SaveDetails saveDetails)
    {
        return new()
        {
            Filename = filename,
            PowerCircuits = saveDetails.PowerCircuits,
            Factories = saveDetails.Factories,
            Stations = saveDetails.Stations,
            Uploaders = saveDetails.Uploaders,
            Resources = saveDetails.Resources,
        };
    }
}

