using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Services;

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
            var inAnyRange = _filters.SelectedLeftoverRanges
                .Any(r => leftover >= r.Min && leftover <= r.Max);
            if (!inAnyRange)
            {
                return false;
            }
        }

        if (_filters.SelectedMaxValues.Count > 0)
        {
            var maxValue = (int)resource.Max;
            if (!_filters.SelectedMaxValues.Contains(maxValue))
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

            return SatisfactoryApp.Utils.Resources.Types
                .Select(type => new ResourceTypeOption
                {
                    Title = $"{type} ({counts.GetValueOrDefault(type, 0)})",
                    Value = type
                })
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

    public IReadOnlyList<LeftoverRangeOption> LeftoverOptions { get; } =
    [
        new() { Title = "Left 0 - 299", Min = 0, Max = 299 },
        new() { Title = "Left 300 - 599", Min = 300, Max = 599 },
        new() { Title = "Left 600 - 999", Min = 600, Max = 999 },
        new() { Title = "Left 1000 - 1200", Min = 1000, Max = 1200 },
    ];

    public IReadOnlyCollection<LeftoverRangeOption> SelectedLeftoverRanges
    {
        get => Filters.SelectedLeftoverRanges;
        set
        {
            Filters.SelectedLeftoverRanges = value?.ToList() ?? [];
            NotifyChanged();
        }
    }

    public IReadOnlyCollection<int> SelectedMaxValues
    {
        get => Filters.SelectedMaxValues;
        set
        {
            Filters.SelectedMaxValues = value?.ToList() ?? [];
            NotifyChanged();
        }
    }
}

public class ResourceFilters
{
    public List<ResourceTypeOption> SelectedResourceTypes { get; set; } = [];
    public List<LeftoverRangeOption> SelectedLeftoverRanges { get; set; } = [];
    public List<int> SelectedMaxValues { get; set; } = [];
}

public class ResourceTypeOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public override string ToString() => Title;
}

public class LeftoverRangeOption
{
    public string Title { get; set; } = string.Empty;
    public float Min { get; set; }
    public float Max { get; set; }

    public override string ToString() => Title;
}
