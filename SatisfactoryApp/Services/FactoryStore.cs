using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Services;

public class FactoryStore
{
    private readonly List<Factory> _factories = [];
    private readonly FactoryFilters _filters = new();
    private int _updateCounter = 0;

    public List<Factory> Factories => _factories;
    public FactoryFilters Filters => _filters;
    public int UpdateCounter => _updateCounter;

    public event Action? FactoriesChanged;
    public event Action? FilteredFactoriesChanged;

    public void SetFactories(List<Factory> factories)
    {
        _factories.Clear();
        _factories.AddRange(factories);

        _updateCounter++;
        FactoriesChanged?.Invoke();
    }

    public void NotifyFiltersChanged()
    {
        _updateCounter++;
        FilteredFactoriesChanged?.Invoke();
    }

    public List<Factory> FilteredFactories => [.. _factories.Where(IsFactoryFiltered)];
    public List<Factory> NonFilteredFactories => [.. _factories.Where(f => !IsFactoryFiltered(f))];

    private bool IsFactoryFiltered(Factory factory)
    {
        if (_filters.SelectedFactoryTypes.Count > 0)
        {
            var factoryType = factory.Type ?? "unknown";
            if (!_filters.SelectedFactoryTypes.Any(f => f.Value == factoryType))
            {
                return true;
            }
        }

        if (_filters.SelectedPowerCircuits.Count > 0)
        {
            var mainCircuitKey = $"main_{factory.MainPowerCircuitId}";
            var subCircuitKey = $"sub_{factory.SubPowerCircuitId}";

            var hasMatchingCircuit =
                (mainCircuitKey != null && _filters.SelectedPowerCircuits.Any(c => c.Value == mainCircuitKey)) ||
                (subCircuitKey != null && _filters.SelectedPowerCircuits.Any(c => c.Value == subCircuitKey));

            if (!hasMatchingCircuit)
            {
                return true;
            }
        }

        if (_filters.SelectedFactoryStabilities.Count > 0)
        {
            var status = GetFactoryStability(factory);
            if (!_filters.SelectedFactoryStabilities.Any(s => s.Value == status))
            {
                return true;
            }
        }

        return false;
    }

    private string GetFactoryStability(Factory factory)
    {
        var percentage = factory.PercentageProducing;
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
                mainCircuitCounts.TryGetValue(factory.MainPowerCircuitId, out var count);
                mainCircuitCounts[factory.MainPowerCircuitId] = count + 1;

                subCircuitCounts.TryGetValue(factory.SubPowerCircuitId, out var count2);
                subCircuitCounts[factory.SubPowerCircuitId] = count2 + 1;
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

    public List<FactoryStabilityOption> FactoryStabilityOptions
    {
        get
        {
            var statusCounts = new Dictionary<string, int>();
            foreach (var factory in _factories)
            {
                var status = GetFactoryStability(factory);
                statusCounts.TryGetValue(status, out var count);
                statusCounts[status] = count + 1;
            }

            return new List<FactoryStabilityOption>
            {
                new() { Title = $"Stable ({statusCounts.GetValueOrDefault("Stable", 0)})", Value = "Stable" },
                new() { Title = $"Almost Stable ({statusCounts.GetValueOrDefault("Almost Stable", 0)})", Value = "Almost Stable" },
                new() { Title = $"Unstable ({statusCounts.GetValueOrDefault("Unstable", 0)})", Value = "Unstable" },
                new() { Title = $"Off ({statusCounts.GetValueOrDefault("Off", 0)})", Value = "Off" },
            };
        }
    }


    public IEnumerable<FactoryTypeOption> SelectedFactoryTypes
    {
        get => Filters.SelectedFactoryTypes;
        set
        {
            Filters.SelectedFactoryTypes = value.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

    public IEnumerable<PowerCircuitOption> SelectedPowerCircuits
    {
        get => Filters.SelectedPowerCircuits;
        set
        {
            Filters.SelectedPowerCircuits = value.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

    public IEnumerable<FactoryStabilityOption> SelectedFactoryStabilities
    {
        get => Filters.SelectedFactoryStabilities;
        set
        {
            Filters.SelectedFactoryStabilities = value.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

}

public class FactoryFilters
{
    public List<FactoryTypeOption> SelectedFactoryTypes { get; set; } = [];
    public List<PowerCircuitOption> SelectedPowerCircuits { get; set; } = [];
    public List<FactoryStabilityOption> SelectedFactoryStabilities { get; set; } = [];
}

public class FactoryTypeOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public override string ToString() => Title;
}

public class PowerCircuitOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public override string ToString() => Title;
}

public class FactoryStabilityOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public override string ToString() => Title;
}

