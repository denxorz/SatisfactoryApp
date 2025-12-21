namespace SatisfactoryApp.Services.Stations;

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
