namespace SatisfactoryApp.Utils;

public static class Resources
{
    public static IReadOnlyList<string> Types { get; } =
    [
        "Iron",
        "Copper",
        "Limestone",
        "Coal",
        "Caterium",
        "Quartz",
        "Sulfur",
        "Bauxite",
        "Uranium",
        "Sam",
        "Oil",
        "Water",
        "Nitrogengas"
    ];

    public static string GetResourceColor(string? resourceType)
    {
        return resourceType switch
        {
            "Iron" => "#808080",
            "Copper" => "#CD7F32",
            "Limestone" => "#C0C0C0",
            "Coal" => "#000000",
            "Caterium" => "#FFD700",
            "Quartz" => "#E0E0E0",
            "Sulfur" => "#FFFF00",
            "Bauxite" => "#8B4513",
            "Uranium" => "#00FF00",
            "Sam" => "#9932CC",
            "Oil" => "#000080",
            "Water" => "#1E90FF",
            "Nitrogengas" => "#87CEEB",
            _ => "red"
        };
    }
}
