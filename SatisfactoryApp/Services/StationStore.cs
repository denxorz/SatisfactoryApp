using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Services;

public class StationStore
{
    private List<Station> _stations = [];
    private List<Uploader> _uploaders = [];
    private readonly StationFilters _filters = new();
    private int _updateCounter = 0;

    public List<Station> Stations => _stations;
    public List<Uploader> Uploaders => _uploaders;
    public StationFilters Filters => _filters;
    public int UpdateCounter => _updateCounter;

    public event Action? StationsChanged;
    public event Action? FilteredStationsChanged;

    public void SetStations(List<Station> stations)
    {
        _stations = stations;
        _updateCounter++;

        StationsChanged?.Invoke();
        FilteredStationsChanged?.Invoke();
    }

    public void SetUploaders(List<Uploader> uploaders)
    {
        _uploaders = uploaders;
        _updateCounter++;
    }

    public void NotifyFiltersChanged()
    {
        _updateCounter++;
        FilteredStationsChanged?.Invoke();
    }

    public List<Station> FilteredStations
    {
        get
        {
            return _stations.Where(s => IsStationFiltered(s)).ToList();
        }
    }

    public List<Uploader> FilteredUploaders
    {
        get
        {
            if (!_filters.ShowUploaders)
            {
                return new List<Uploader>();
            }

            return _uploaders.Where(u => IsUploaderFiltered(u)).ToList();
        }
    }

    private bool IsStationFiltered(Station station)
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

        if (_filters.SelectedStationTypes.Count > 0)
        {
            Console.WriteLine($"IsStationFiltered: {station.Type}");
            if (!_filters.SelectedStationTypes.Contains(station.Type, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        if (_filters.SelectedTransferTypes.Count > 0)
        {
            var transferType = station.IsUnload ? "unload" : "load";
            if (!_filters.SelectedTransferTypes.Contains(transferType, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }
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

    public List<CargoTypeOption> CargoTypeOptions
    {
        get
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

            return cargoTypes
                .OrderBy(t => t)
                .Select(t => new CargoTypeOption { Title = t, Value = t })
                .ToList();
        }
    }

    public IReadOnlyCollection<string> SelectedTransferTypes
    {
        get => Filters.SelectedTransferTypes;
        set
        {
            Filters.SelectedTransferTypes = value.ToList() ?? [];
            NotifyFiltersChanged();
        }
    }

    public IReadOnlyCollection<string> SelectedStationTypes
    {
        get => Filters.SelectedStationTypes;
        set
        {
            Filters.SelectedStationTypes = value.ToList() ?? [];
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
}

public class StationFilters
{
    public string SearchText { get; set; } = string.Empty;
    public List<string> SelectedStationTypes { get; set; } = [];
    public List<string> SelectedTransferTypes { get; set; } = [];
    public List<CargoTypeOption> SelectedCargoTypes { get; set; } = [];
    public bool ShowUploaders { get; set; } = true;
}

public class CargoTypeOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public override string ToString() => Title;
}

