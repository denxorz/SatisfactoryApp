using System.Net.Http.Json;
using System.Text.Json;
using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Services;

public class TestDataService
{
    private const string TestDataFileName = "testdata.json";

    public static async Task<(List<Factory> Factories, List<PowerCircuit> PowerCircuits, List<Station> Stations, List<Uploader> Uploaders, List<Resource> Resources)> GetTestDataAsync(HttpClient httpClient)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var testData = await httpClient.GetFromJsonAsync<TestDataContainer>(TestDataFileName, options)
            ?? throw new InvalidOperationException($"Failed to load test data from {TestDataFileName}");

        return (testData.Factories, testData.PowerCircuits, testData.Stations, testData.Uploaders, testData.Resources);
    }

    public static string ExportToJson(FactoryStore factoryStore, StationStore stationStore, ResourceStore resourceStore)
    {
        var testData = new TestDataContainer
        {
            PowerCircuits = factoryStore.PowerCircuits.ToList(),
            Factories = factoryStore.Factories.ToList(),
            Stations = stationStore.Stations.ToList(),
            Uploaders = stationStore.Uploaders.ToList(),
            Resources = resourceStore.Resources.ToList()
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(testData, options);
    }
}

internal class TestDataContainer
{
    public List<PowerCircuit> PowerCircuits { get; set; } = new();
    public List<Factory> Factories { get; set; } = new();
    public List<Station> Stations { get; set; } = new();
    public List<Uploader> Uploaders { get; set; } = new();
    public List<Resource> Resources { get; set; } = new();
}

