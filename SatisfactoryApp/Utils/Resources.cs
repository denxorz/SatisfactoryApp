namespace SatisfactoryApp.Utils;

public static class Resources
{
    public static IReadOnlyList<string> Types { get; } =
    [
        "OreIron",
        "OreCopper",
        "Stone",
        "Coal",
        "OreGold",
        "RawQuartz",
        "Sulfur",
        "OreBauxite",
        "OreUranium",
        "SAM",
        "LiquidOil",
        "Water",
        "Nitrogengas"
    ];

    public static string GetResourceColor(string? resourceType)
    {
        return resourceType switch
        {
            "OreIron" => "#808080",
            "OreCopper" => "#CD7F32",
            "Stone" => "#C0C0C0",
            "Coal" => "#000000",
            "OreGold" => "#FFD700",
            "RawQuartz" => "#E0E0E0",
            "Sulfur" => "#FFFF00",
            "OreBauxite" => "#8B4513",
            "OreUranium" => "#00FF00",
            "SAM" => "#9932CC",
            "LiquidOil" => "#000080",
            "Water" => "#1E90FF",
            "Nitrogengas" => "#87CEEB",
            _ => "red"
        };
    }
}
