using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Services;

public class StationStore
{
    private List<Station> _stations = new();
    private List<Uploader> _uploaders = new();
    private StationFilters _filters = new();
    private int _updateCounter = 0;

    public List<Station> Stations => _stations;
    public List<Uploader> Uploaders => _uploaders;
    public StationFilters Filters => _filters;
    public int UpdateCounter => _updateCounter;

    public void SetStations(List<Station> stations)
    {
        _stations = stations;
        _updateCounter++;
    }

    public void SetUploaders(List<Uploader> uploaders)
    {
        _uploaders = uploaders;
        _updateCounter++;
    }

    public void NotifyFiltersChanged()
    {
        _updateCounter++;
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
            var stationType = station.Type ?? "unknown";
            if (!_filters.SelectedStationTypes.Contains(stationType))
            {
                return false;
            }
        }

        if (_filters.SelectedTransferTypes.Count > 0)
        {
            var transferType = station.IsUnload ? "unload" : "load";
            if (!_filters.SelectedTransferTypes.Contains(transferType))
            {
                return false;
            }
        }

        if (_filters.SelectedCargoTypes.Count > 0)
        {
            var stationCargoTypes = station.CargoTypes ?? new List<string>();
            var hasMatchingCargo = _filters.SelectedCargoTypes.Any(selectedType => stationCargoTypes.Contains(selectedType));
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
            var uploaderCargoTypes = uploader.CargoTypes ?? new List<string>();
            var hasMatchingCargo = _filters.SelectedCargoTypes.Any(selectedType => uploaderCargoTypes.Contains(selectedType));
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

            foreach (var station in _stations)
            {
                if (station.CargoTypes != null)
                {
                    foreach (var type in station.CargoTypes)
                    {
                        if (!string.IsNullOrEmpty(type))
                        {
                            cargoTypes.Add(type);
                        }
                    }
                }
            }

            foreach (var uploader in _uploaders)
            {
                if (uploader.CargoTypes != null)
                {
                    foreach (var type in uploader.CargoTypes)
                    {
                        if (!string.IsNullOrEmpty(type))
                        {
                            cargoTypes.Add(type);
                        }
                    }
                }
            }

            return cargoTypes
                .OrderBy(t => t)
                .Select(t => new CargoTypeOption { Title = t, Value = t })
                .ToList();
        }
    }
}

public class StationFilters
{
    public string SearchText { get; set; } = string.Empty;
    public List<string> SelectedStationTypes { get; set; } = new();
    public List<string> SelectedTransferTypes { get; set; } = new();
    public List<string> SelectedCargoTypes { get; set; } = new();
    public bool ShowUploaders { get; set; } = true;
}

public class CargoTypeOption
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

