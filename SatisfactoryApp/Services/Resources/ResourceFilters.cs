namespace SatisfactoryApp.Services.Resources;

public class ResourceFilters
{
    public List<ResourceTypeOption> SelectedResourceTypes { get; set; } = [];
    public List<LeftoverRangeOption> SelectedLeftoverRanges { get; set; } = [];
    public List<LeftoverRangeOption> AvailableAfterFilterLeftoverOptions { get; set; } = [];
    public List<int> SelectedMaxValues { get; set; } = [];
    public List<int> AvailableAfterFilterMaxValues { get; set; } = [];
}
