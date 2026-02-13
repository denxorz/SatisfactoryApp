using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Services.Factories;

public class FactoryStore
{
    private readonly List<Factory> _factories = [];
    private readonly List<PowerCircuit> _powerCircuits = [];
    private Dictionary<int, PowerCircuit> _powerCircuitsById = [];
    private readonly FactoryFilters _filters = new();
    private List<Factory> _filteredFactories = [];
    private List<FactoryTypeOption> _factoryTypeOptions = [];
    private List<PowerCircuitOption> _powerCircuitIdOptions = [];
    private List<FactoryStabilityOption> _factoryStabilityOptions = [];
    private HashSet<string>? _selectedFactoryTypes;
    private HashSet<string>? _selectedPowerCircuits;
    private HashSet<string>? _selectedFactoryStabilities;
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
        _powerCircuitsById = new Dictionary<int, PowerCircuit>();
        
        foreach (var c in _powerCircuits)
        {
            _powerCircuitsById.TryAdd(c.Id, c);
        }

        UpdateOptions();

        FactoriesChanged?.Invoke();
        NotifyFiltersChanged();
    }

    private void NotifyFiltersChanged()
    {
        UpdateFilterCaches();
        _filteredFactories = [.. _factories.Where(IsFactoryIncluded)];
        _updateCounter++;
        FilteredFactoriesChanged?.Invoke();
    }

    public List<Factory> FilteredFactories => _filteredFactories;

    public PowerCircuit? GetCircuitById(int id)
    {
        return _powerCircuitsById.GetValueOrDefault(id);
    }

    private bool IsFactoryIncluded(Factory factory)
    {
        if (_selectedFactoryTypes is { Count: > 0 })
        {
            var factoryType = factory.Type ?? "unknown";
            if (!_selectedFactoryTypes.Contains(factoryType))
            {
                return false;
            }
        }

        if (_selectedPowerCircuits is { Count: > 0 })
        {
            var mainCircuitKey = $"main_{factory.MainPowerCircuitId}";
            var subCircuitKey = $"sub_{factory.SubPowerCircuitId}";

            var hasMatchingCircuit = _selectedPowerCircuits.Contains(mainCircuitKey) 
                                     || _selectedPowerCircuits.Contains(subCircuitKey);

            if (!hasMatchingCircuit)
            {
                return false;
            }
        }

        if (_selectedFactoryStabilities is { Count: > 0 })
        {
            var status = GetFactoryStability(factory);
            if (!_selectedFactoryStabilities.Contains(status))
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
        get => _factoryTypeOptions;
    }

    public List<PowerCircuitOption> PowerCircuitIdOptions
    {
        get => _powerCircuitIdOptions;
    }

    public List<FactoryStabilityOption> FactoryStabilityOptions
    {
        get => _factoryStabilityOptions;
    }

    public IEnumerable<FactoryTypeOption> SelectedFactoryTypes
    {
        get => Filters.SelectedFactoryTypes;
        set
        {
            Filters.SelectedFactoryTypes = value?.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

    public IEnumerable<PowerCircuitOption> SelectedPowerCircuits
    {
        get => Filters.SelectedPowerCircuits;
        set
        {
            Filters.SelectedPowerCircuits = value?.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

    public IEnumerable<FactoryStabilityOption> SelectedFactoryStabilities
    {
        get => Filters.SelectedFactoryStabilities;
        set
        {
            Filters.SelectedFactoryStabilities = value?.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

    private void UpdateFilterCaches()
    {
        _selectedFactoryTypes = _filters.SelectedFactoryTypes.Count > 0
            ? _filters.SelectedFactoryTypes.Select(f => f.Value).ToHashSet()
            : null;
        _selectedPowerCircuits = _filters.SelectedPowerCircuits.Count > 0
            ? _filters.SelectedPowerCircuits.Select(c => c.Value).ToHashSet()
            : null;
        _selectedFactoryStabilities = _filters.SelectedFactoryStabilities.Count > 0
            ? _filters.SelectedFactoryStabilities.Select(s => s.Value).ToHashSet()
            : null;
    }

    private void UpdateOptions()
    {
        var factoryTypeCounts = new Dictionary<string, int>();
        var statusCounts = new Dictionary<string, int>();
        var mainCircuitCounts = new Dictionary<int, int>();
        var subCircuitCounts = new Dictionary<int, int>();

        foreach (var factory in _factories)
        {
            if (!string.IsNullOrEmpty(factory.Type))
            {
                factoryTypeCounts.TryGetValue(factory.Type, out var count);
                factoryTypeCounts[factory.Type] = count + 1;
            }

            var status = GetFactoryStability(factory);
            statusCounts.TryGetValue(status, out var statusCount);
            statusCounts[status] = statusCount + 1;

            mainCircuitCounts.TryGetValue(factory.MainPowerCircuitId, out var mainCount);
            mainCircuitCounts[factory.MainPowerCircuitId] = mainCount + 1;

            subCircuitCounts.TryGetValue(factory.SubPowerCircuitId, out var subCount);
            subCircuitCounts[factory.SubPowerCircuitId] = subCount + 1;
        }

        _factoryTypeOptions = factoryTypeCounts
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => new FactoryTypeOption($"{kvp.Key} ({kvp.Value})", kvp.Key))
            .ToList();

        var mainOptions = _powerCircuits
            .Where(c => c.ParentCircuitId is null)
            .OrderBy(c => c.Id)
            .Select(c => new PowerCircuitOption(CircuitNames.GetFilterName(c, mainCircuitCounts.GetValueOrDefault(c.Id, 0)), $"main_{c.Id}"))
            .ToList();

        var subOptions = _powerCircuits
            .Where(c => c.ParentCircuitId is not null)
            .OrderBy(c => c.Id)
            .Select(c => new PowerCircuitOption(CircuitNames.GetFilterName(c, subCircuitCounts.GetValueOrDefault(c.Id, 0)), $"sub_{c.Id}"))
            .ToList();

        _powerCircuitIdOptions = [.. mainOptions, .. subOptions];

        _factoryStabilityOptions =
        [
            new($"Stable ({statusCounts.GetValueOrDefault("Stable", 0)})", "Stable"),
            new($"Almost Stable ({statusCounts.GetValueOrDefault("Almost Stable", 0)})", "Almost Stable"),
            new($"Unstable ({statusCounts.GetValueOrDefault("Unstable", 0)})", "Unstable"),
            new($"Off ({statusCounts.GetValueOrDefault("Off", 0)})", "Off"),
            new($"Unknown ({statusCounts.GetValueOrDefault("Unknown", 0)})", "Unknown"),
        ];
    }
}
