using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Services.Factories;

public class FactoryStore
{
    private readonly List<Factory> _factories = [];
    private readonly List<PowerCircuit> _powerCircuits = [];
    private readonly FactoryFilters _filters = new();
    private int _updateCounter = 0;

    public List<Factory> Factories => _factories;
    public List<PowerCircuit> PowerCircuits => _powerCircuits;
    public FactoryFilters Filters => _filters;
    public int UpdateCounter => _updateCounter;

    public event Action? FactoriesChanged;
    public event Action? FilteredFactoriesChanged;

    public void Set(List<Factory> factories, List<PowerCircuit> powerCircuits)
    {
        _factories.Clear();
        _factories.AddRange(factories);

        _powerCircuits.Clear();
        _powerCircuits.AddRange(powerCircuits);

        FactoriesChanged?.Invoke();
        NotifyFiltersChanged();
    }

    public void NotifyFiltersChanged()
    {
        _updateCounter++;
        FilteredFactoriesChanged?.Invoke();
    }

    public List<Factory> FilteredFactories => [.. _factories.Where(IsFactoryIncluded)];

    private bool IsFactoryIncluded(Factory factory)
    {
        if (_filters.SelectedFactoryTypes.Count > 0)
        {
            var factoryType = factory.Type ?? "unknown";
            if (!_filters.SelectedFactoryTypes.Any(f => f.Value == factoryType))
            {
                return false;
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
                return false;
            }
        }

        if (_filters.SelectedFactoryStabilities.Count > 0)
        {
            var status = GetFactoryStability(factory);
            if (!_filters.SelectedFactoryStabilities.Any(s => s.Value == status))
            {
                return false;
            }
        }

        return true;
    }

    private static string GetFactoryStability(Factory factory)
    {
        var percentage = factory.PercentageProducing;
        if (percentage is null) return "Unknown";
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
                .Select(kvp => new FactoryTypeOption($"{kvp.Key} ({kvp.Value})", kvp.Key))
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

            var mainOptions = _powerCircuits
                .Where(c => c.ParentCircuitId is null)
                .OrderBy(c => c.Id)
                .Select(c => new PowerCircuitOption(CircuitNames.GetFilterName(c, mainCircuitCounts.TryGetValue(c.Id, out var count) ? count : 0), $"main_{c.Id}"))
                .ToList();

            var subOptions = _powerCircuits
                .Where(c => c.ParentCircuitId is not null)
                .OrderBy(c => c.Id)
                .Select(c => new PowerCircuitOption(CircuitNames.GetFilterName(c, subCircuitCounts.TryGetValue(c.Id, out var count) ? count : 0), $"sub_{c.Id}"))
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

            return
            [
                new($"Stable ({statusCounts.GetValueOrDefault("Stable", 0)})", "Stable"),
                new($"Almost Stable ({statusCounts.GetValueOrDefault("Almost Stable", 0)})", "Almost Stable"),
                new($"Unstable ({statusCounts.GetValueOrDefault("Unstable", 0)})", "Unstable"),
                new($"Off ({statusCounts.GetValueOrDefault("Off", 0)})", "Off"),
                new($"Unknown ({statusCounts.GetValueOrDefault("Unknown", 0)})", "Unknown"),
            ];
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
