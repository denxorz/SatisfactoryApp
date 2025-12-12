namespace SatisfactoryApp.Utils;

public static class FactoryColors
{
    private static readonly string[] PowerCircuitColors = new[]
    {
        "#FF0000", "#008000", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF",
        "#FFA500", "#A52A2A", "#00FF00", "#FFD700", "#40E0D0", "#EE82EE",
        "#800000", "#228B22", "#4169E1", "#FF1493", "#ADFF2F", "#DC143C",
        "#000080", "#FF4500", "#9370DB", "#3CB371", "#FF7F50", "#008080",
        "#708090", "#808000", "#4B0082", "#C0C0C0", "#F0E68C", "#DDA0DD",
    };

    public static string GetPowerCircuitColor(int? powerCircuitId)
    {
        if (!powerCircuitId.HasValue) return "#D3D3D3";
        if (powerCircuitId.Value < 0) return "#808080";
        var circuitIndex = powerCircuitId.Value % PowerCircuitColors.Length;
        return PowerCircuitColors[circuitIndex];
    }

    public static string GetFactoryColorForPowerCircuit(int? subPowerCircuitId)
    {
        return GetPowerCircuitColor(subPowerCircuitId);
    }

    public static string GetFactoryColorForStability(int? percentageProducing)
    {
        if (percentageProducing is null) return "#808080";
        if (percentageProducing == 100) return "#00FF00";
        if (percentageProducing >= 95 && percentageProducing < 100) return "#FFFF00";
        if (percentageProducing >= 1 && percentageProducing < 95) return "#FFA500";
        return "#FF0000";
    }
}

