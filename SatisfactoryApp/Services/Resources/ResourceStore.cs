using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Services.Resources;

public class ResourceStore
{
    private readonly List<Resource> _resources = [];
    private readonly ResourceFilters _filters = new();
    private int _updateCounter = 0;

    public List<Resource> Resources => _resources;
    public List<Resource> FilteredResources => [.. _resources.Where(IsResourceIncluded)];
    public ResourceFilters Filters => _filters;
    public int UpdateCounter => _updateCounter;

    public event Action? ResourcesChanged;

    public void Set(List<Resource> resources)
    {
        _resources.Clear();
        _resources.AddRange(resources);

        NotifyChanged();
    }

    private void NotifyChanged()
    {
        _updateCounter++;
        ResourcesChanged?.Invoke();
    }

    private bool IsResourceIncluded(Resource resource)
    {
        if (_filters.SelectedResourceTypes.Count > 0)
        {
            var type = resource.Type ?? "Unknown";
            if (!_filters.SelectedResourceTypes.Any(t =>
                    string.Equals(t.Value, type, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
        }

        if (_filters.SelectedLeftoverRanges.Count > 0)
        {
            var leftover = resource.Max - resource.Flow;
            var inAnyRange = _filters.AvailableAfterFilterLeftoverOptions
                .Any(r => leftover >= r.Min && leftover <= r.Max);
            if (!inAnyRange)
            {
                return false;
            }
        }

        if (_filters.SelectedMaxValues.Count > 0)
        {
            if (!_filters.AvailableAfterFilterMaxValues.Contains(resource.Max))
            {
                return false;
            }
        }

        return true;
    }

    public List<ResourceTypeOption> ResourceTypeOptions
    {
        get
        {
            var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var resource in _resources)
            {
                var type = resource.Type ?? "Unknown";
                counts.TryGetValue(type, out var count);
                counts[type] = count + 1;
            }

            return Utils.Resources.Types
                    .Select(type => new ResourceTypeOption($"{type} ({counts.GetValueOrDefault(type, 0)})", type))
                    .ToList();
        }
    }

    public IEnumerable<ResourceTypeOption> SelectedResourceTypes
    {
        get => Filters.SelectedResourceTypes;
        set
        {
            Filters.SelectedResourceTypes = value?.ToList() ?? [];
            NotifyChanged();
        }
    }

    public IReadOnlyList<LeftoverRangeOption> AllLeftoverOptions { get; } =
    [
        new("Left 0 - 299", 0, 299),
        new("Left 300 - 599", 300, 599),
        new("Left 600 - 999", 600, 999),
        new("Left 1000 - 1200", 1000, 1200),
    ];

    public IReadOnlyCollection<LeftoverRangeOption> SelectedLeftoverRanges
    {
        get => Filters.SelectedLeftoverRanges;
        set
        {
            Filters.SelectedLeftoverRanges = value?.ToList() ?? [];
            Filters.AvailableAfterFilterLeftoverOptions = [.. AllLeftoverOptions.Except(_filters.SelectedLeftoverRanges)];
            NotifyChanged();
        }
    }

    public IReadOnlyCollection<int> SelectedMaxValues
    {
        get => Filters.SelectedMaxValues;
        set
        {
            Filters.SelectedMaxValues = value?.ToList() ?? [];
            Filters.AvailableAfterFilterMaxValues = [.. ((int[])[300, 600, 1200]).Except(_filters.SelectedMaxValues)];
            NotifyChanged();
        }
    }
}
