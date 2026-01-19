using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Services.Stations;

public class StationStore
{
    private List<Station> _stations = [];
    private List<Uploader> _uploaders = [];
    private readonly StationFilters _filters = new();
    private List<Station> _filteredStations = [];
    private List<Uploader> _filteredUploaders = [];
    private string? _searchText;
    private HashSet<string>? _availableStationTypes;
    private HashSet<string>? _availableTransferTypes;
    private HashSet<string>? _selectedCargoTypes;
    private HashSet<string>? _selectedCargoTypesIgnoreCase;
    private int _updateCounter = 0;

    public List<Station> Stations => _stations;
    public List<Station> FilteredStations => _filteredStations;
    public List<Uploader> Uploaders => _uploaders;
    public StationFilters Filters => _filters;
    public List<CargoTypeOption> CargoTypeOptions { get; private set; } = [];
    public int UpdateCounter => _updateCounter;

    public event Action? StationsChanged;
    public event Action? FilteredStationsChanged;

    public void Set(List<Station> stations, List<Uploader> uploaders)
    {
        _stations = stations;
        _uploaders = uploaders;

        CargoTypeOptions = GetAvailableCargoTypes();

        StationsChanged?.Invoke();
        NotifyFiltersChanged();
    }

    public void NotifyFiltersChanged()
    {
        UpdateFilterCaches();
        UpdateFilteredStations();
        UpdateFilteredUploaders();
        _updateCounter++;
        FilteredStationsChanged?.Invoke();
    }

    public List<Uploader> FilteredUploaders
    {
        get => _filteredUploaders;
    }

    private bool IsIncluded(Station station)
    {
        if (!string.IsNullOrEmpty(_searchText))
        {
            var stationName = station.Name ?? string.Empty;
            var stationShortName = station.ShortName ?? string.Empty;

            if (!stationName.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                && !stationShortName.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        if (_filters.SelectedStationTypes.Count > 0
            && (_availableStationTypes == null || !_availableStationTypes.Contains(station.Type ?? string.Empty)))
        {
            return false;
        }

        if (_filters.SelectedTransferTypes.Count > 0
            && (_availableTransferTypes == null || !_availableTransferTypes.Contains(station.IsUnload ? "Unload" : "Load")))
        {
            return false;
        }

        if (_selectedCargoTypesIgnoreCase is { Count: > 0 })
        {
            if (!station.CargoTypes.Any(c => _selectedCargoTypesIgnoreCase.Contains(c)))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsUploaderFiltered(Uploader uploader)
    {
        if (_selectedCargoTypes is { Count: > 0 })
        {
            if (!uploader.CargoTypes.Any(c => _selectedCargoTypes.Contains(c)))
            {
                return false;
            }
        }

        return true;
    }

    public string SearchText
    {
        get => Filters.SearchText;
        set
        {
            Filters.SearchText = value;
            NotifyFiltersChanged();
        }
    }

    public IReadOnlyCollection<string> SelectedTransferTypes
    {
        get => Filters.SelectedTransferTypes;
        set
        {
            Filters.SelectedTransferTypes = value?.ToList() ?? [];
            Filters.AvailableAfterFilterTransferTypes = [.. Filters.AllTransferTypes.Except(_filters.SelectedTransferTypes)];
            NotifyFiltersChanged();
        }
    }

    public IReadOnlyCollection<string> SelectedStationTypes
    {
        get => Filters.SelectedStationTypes;
        set
        {
            Filters.SelectedStationTypes = value?.ToList() ?? [];
            Filters.AvailableAfterFilterStationTypes = [.. Filters.AllStationTypes.Except(_filters.SelectedStationTypes)];
            NotifyFiltersChanged();
        }
    }

    public IEnumerable<CargoTypeOption> SelectedCargoTypes
    {
        get => Filters.SelectedCargoTypes;
        set
        {
            Filters.SelectedCargoTypes = value?.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

    private List<CargoTypeOption> GetAvailableCargoTypes()
    {
        var cargoTypes = new HashSet<string>();

        foreach (var c in _stations.SelectMany(s => s.CargoTypes))
        {
            cargoTypes.Add(c);
        }

        foreach (var c in _uploaders.SelectMany(s => s.CargoTypes))
        {
            cargoTypes.Add(c);
        }

        return [.. cargoTypes
            .OrderBy(t => t)
            .Select(t => new CargoTypeOption(t, t))];
    }

    private void UpdateFilterCaches()
    {
        _searchText = string.IsNullOrWhiteSpace(_filters.SearchText) ? null : _filters.SearchText;
        _availableStationTypes = _filters.AvailableAfterFilterStationTypes.Count > 0
            ? _filters.AvailableAfterFilterStationTypes.ToHashSet(StringComparer.OrdinalIgnoreCase)
            : null;
        _availableTransferTypes = _filters.AvailableAfterFilterTransferTypes.Count > 0
            ? _filters.AvailableAfterFilterTransferTypes.ToHashSet(StringComparer.OrdinalIgnoreCase)
            : null;

        if (_filters.SelectedCargoTypes.Count > 0)
        {
            var values = _filters.SelectedCargoTypes.Select(s => s.Value);
            _selectedCargoTypes = new HashSet<string>(values, StringComparer.Ordinal);
            _selectedCargoTypesIgnoreCase = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
        }
        else
        {
            _selectedCargoTypes = null;
            _selectedCargoTypesIgnoreCase = null;
        }
    }

    private void UpdateFilteredStations()
    {
        _filteredStations = [.. _stations.Where(IsIncluded)];
    }

    private void UpdateFilteredUploaders()
    {
        if (_filters.SelectedTransferTypes.Count > 0
            && (_availableTransferTypes == null || !_availableTransferTypes.Contains("Uploader")))
        {
            _filteredUploaders = [];
            return;
        }

        _filteredUploaders = [.. _uploaders.Where(IsUploaderFiltered)];
    }
}
