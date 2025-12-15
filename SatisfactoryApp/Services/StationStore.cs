using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Services;

public class StationStore
{
    private List<Station> _stations = [];
    private List<Uploader> _uploaders = [];
    private readonly StationFilters _filters = new();
    private int _updateCounter = 0;

    public List<Station> Stations => _stations;
    public List<Station> FilteredStations => [.. _stations.Where(IsIncluded)];
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
        _updateCounter++;
        FilteredStationsChanged?.Invoke();
    }

    public List<Uploader> FilteredUploaders
    {
        get
        {
            if (_filters.SelectedTransferTypes.Count > 0 && !_filters.AvailableAfterFilterTransferTypes.Contains("Uploader"))
            {
                return [];
            }

            return [.. _uploaders.Where(IsUploaderFiltered)];
        }
    }

    private bool IsIncluded(Station station)
    {
        if (!string.IsNullOrEmpty(_filters.SearchText))
        {
            var searchText = _filters.SearchText.ToLower();
            var stationName = (station.Name ?? "").ToLower();
            var stationShortName = (station.ShortName ?? "").ToLower();

            if (!stationName.Contains(searchText) && !stationShortName.Contains(searchText))
            {
                return false;
            }
        }

        if (_filters.SelectedStationTypes.Count > 0
            && !_filters.AvailableAfterFilterStationTypes.Contains(station.Type, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        if (_filters.SelectedTransferTypes.Count > 0
            && !_filters.AvailableAfterFilterTransferTypes.Contains(station.IsUnload ? "Unload" : "Load"))
        {
            return false;
        }

        if (_filters.SelectedCargoTypes.Count > 0)
        {
            var hasMatchingCargo = _filters.SelectedCargoTypes.Any(s => station.CargoTypes.Contains(s.Value, StringComparer.OrdinalIgnoreCase));
            if (!hasMatchingCargo)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsUploaderFiltered(Uploader uploader)
    {
        if (_filters.SelectedCargoTypes.Count > 0)
        {
            var hasMatchingCargo = _filters.SelectedCargoTypes.Any(s => uploader.CargoTypes.Contains(s.Value));
            if (!hasMatchingCargo)
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
            Filters.SelectedTransferTypes = value.ToList() ?? [];
            Filters.AvailableAfterFilterTransferTypes = [.. Filters.AllTransferTypes.Except(_filters.SelectedTransferTypes)];
            NotifyFiltersChanged();
        }
    }

    public IReadOnlyCollection<string> SelectedStationTypes
    {
        get => Filters.SelectedStationTypes;
        set
        {
            Filters.SelectedStationTypes = value.ToList() ?? [];
            Filters.AvailableAfterFilterStationTypes = [.. Filters.AllStationTypes.Except(_filters.SelectedStationTypes)];
            NotifyFiltersChanged();
        }
    }

    public IEnumerable<CargoTypeOption> SelectedCargoTypes
    {
        get => Filters.SelectedCargoTypes;
        set
        {
            Filters.SelectedCargoTypes = value.ToList() ?? [];
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
            .Select(t => new CargoTypeOption { Title = t, Value = t })];
    }
}

public class StationFilters
{
    public string SearchText { get; set; } = string.Empty;

    public List<string> SelectedStationTypes { get; set; } = [];
    public List<string> AvailableAfterFilterStationTypes { get; set; } = [];
    public List<string> AllStationTypes { get; } = ["Train", "Truck", "Drone"];

    public List<string> SelectedTransferTypes { get; set; } = [];
    public List<string> AvailableAfterFilterTransferTypes { get; set; } = [];
    public List<string> AllTransferTypes { get; } = ["Load", "Unload", "Uploader"];

    public List<CargoTypeOption> SelectedCargoTypes { get; set; } = [];
}

public class CargoTypeOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public override string ToString() => Title;
}
