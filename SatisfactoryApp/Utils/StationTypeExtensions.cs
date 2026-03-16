using MudBlazor;

namespace SatisfactoryApp.Utils;

public static class StationTypeExtensions
{
    public static string GetStationTypeIcon(this string? stationType)
    {
        return stationType switch
        {
            "train" => MudBlazor.FontIcons.MaterialSymbols.Outlined.Monorail,
            "truck" => MudBlazor.FontIcons.MaterialSymbols.Outlined.LocalShipping,
            "drone" => MudBlazor.FontIcons.MaterialSymbols.Outlined.Flight,
            _ => MudBlazor.FontIcons.MaterialSymbols.Outlined.Flight
        };
    }

    public static bool IsTrainType(this string? stationType) =>
        stationType == "train";

    public static bool IsTruckType(this string? stationType) =>
        stationType == "truck";

    public static bool IsDroneType(this string? stationType) =>
        stationType == "drone";

    public static (string Style, int PenWidth) GetStationEdgeStyle(this string? stationType)
    {
        return stationType switch
        {
            "drone" => ("dashed", 2),
            "truck" => ("dotted", 4),
            _ => ("solid", 2),
        };
    }
}
