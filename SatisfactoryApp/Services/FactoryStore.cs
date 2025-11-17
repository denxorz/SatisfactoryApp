using SatisfactoryApp.Models;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Services;

public class FactoryStore
{
    private List<Factory> _factories = new();
    private FactoryFilters _filters = new();
    private int _updateCounter = 0;

    public List<Factory> Factories => _factories;
    public FactoryFilters Filters => _filters;
    public int UpdateCounter => _updateCounter;

    public void SetFactories(List<Factory> factories)
    {
        _factories = factories;
        _updateCounter++;
    }

    public List<Factory> FilteredFactories
    {
        get
        {
            return _factories.Where(f => IsFactoryFiltered(f)).ToList();
        }
    }

    public List<Factory> NonFilteredFactories
    {
        get
        {
            return _factories.Where(f => !IsFactoryFiltered(f)).ToList();
        }
    }

    private bool IsFactoryFiltered(Factory factory)
    {
        if (_filters.SelectedFactoryTypes.Count > 0)
        {
            var factoryType = factory.Type ?? "unknown";
            if (!_filters.SelectedFactoryTypes.Contains(factoryType))
            {
                return true;
            }
        }

        if (_filters.SelectedPowerCircuits.Count > 0)
        {
            var mainCircuitKey = factory.MainPowerCircuitId.HasValue ? $"main_{factory.MainPowerCircuitId.Value}" : null;
            var subCircuitKey = factory.SubPowerCircuitId.HasValue ? $"sub_{factory.SubPowerCircuitId.Value}" : null;

            var hasMatchingCircuit =
                (mainCircuitKey != null && _filters.SelectedPowerCircuits.Contains(mainCircuitKey)) ||
                (subCircuitKey != null && _filters.SelectedPowerCircuits.Contains(subCircuitKey));

            if (!hasMatchingCircuit)
            {
                return true;
            }
        }

        if (_filters.SelectedFactoryStabilities.Count > 0)
        {
            var status = GetFactoryStatus(factory);
            if (!_filters.SelectedFactoryStabilities.Contains(status))
            {
                return true;
            }
        }

        return false;
    }

    private string GetFactoryStatus(Factory factory)
    {
        var percentage = factory.PercentageProducing ?? 0;
        if (percentage == 100) return "Stable";
        if (percentage >= 95 && percentage < 100) return "Almost Stable";
        if (percentage >= 1 && percentage < 95) return "Unstable";
        return "Off";
    }

    public List<FactoryTypeOption> FactoryTypeOptions
    {
        get
        {
            var factoryTypeCounts = new Dictionary<string, int>();
            foreach (var factory in _factories)
            {
                if (!string.IsNullOrEmpty(factory.Type))
                {
                    factoryTypeCounts.TryGetValue(factory.Type, out var count);
                    factoryTypeCounts[factory.Type] = count + 1;
                }
            }

            return factoryTypeCounts
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new FactoryTypeOption { Title = $"{kvp.Key} ({kvp.Value})", Value = kvp.Key })
                .ToList();
        }
    }

    public List<PowerCircuitOption> PowerCircuitIdOptions
    {
        get
        {
            var mainCircuitCounts = new Dictionary<int, int>();
            var subCircuitCounts = new Dictionary<int, int>();

            foreach (var factory in _factories)
            {
                if (factory.MainPowerCircuitId.HasValue)
                {
                    mainCircuitCounts.TryGetValue(factory.MainPowerCircuitId.Value, out var count);
                    mainCircuitCounts[factory.MainPowerCircuitId.Value] = count + 1;
                }
                if (factory.SubPowerCircuitId.HasValue)
                {
                    subCircuitCounts.TryGetValue(factory.SubPowerCircuitId.Value, out var count);
                    subCircuitCounts[factory.SubPowerCircuitId.Value] = count + 1;
                }
            }

            var mainOptions = mainCircuitCounts
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new PowerCircuitOption
                {
                    Title = CircuitNames.GetMainCircuitFilterName(kvp.Key, kvp.Value),
                    Value = $"main_{kvp.Key}"
                })
                .ToList();

            var subOptions = subCircuitCounts
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new PowerCircuitOption
                {
                    Title = CircuitNames.GetSubCircuitFilterName(kvp.Key, kvp.Value),
                    Value = $"sub_{kvp.Key}"
                })
                .ToList();

            return mainOptions.Concat(subOptions).ToList();
        }
    }

    public List<FactoryStatusOption> FactoryStatusOptions
    {
        get
        {
            var statusCounts = new Dictionary<string, int>();
            foreach (var factory in _factories)
            {
                var status = GetFactoryStatus(factory);
                statusCounts.TryGetValue(status, out var count);
                statusCounts[status] = count + 1;
            }

            return new List<FactoryStatusOption>
            {
                new() { Title = $"Stable ({statusCounts.GetValueOrDefault("Stable", 0)})", Value = "Stable" },
                new() { Title = $"Almost Stable ({statusCounts.GetValueOrDefault("Almost Stable", 0)})", Value = "Almost Stable" },
                new() { Title = $"Unstable ({statusCounts.GetValueOrDefault("Unstable", 0)})", Value = "Unstable" },
                new() { Title = $"Off ({statusCounts.GetValueOrDefault("Off", 0)})", Value = "Off" },
            };
        }
    }
}

public class FactoryFilters
{
    public List<string> SelectedFactoryTypes { get; set; } = new();
    public List<string> SelectedPowerCircuits { get; set; } = new();
    public List<string> SelectedFactoryStabilities { get; set; } = new();
}

public class FactoryTypeOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class PowerCircuitOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class FactoryStatusOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

