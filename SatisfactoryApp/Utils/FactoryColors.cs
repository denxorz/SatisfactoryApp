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

    public static string GetFactoryColorForClockSpeed(float? clockSpeed)
    {
        if (!clockSpeed.HasValue) return "#808080";
        return GetTemperatureColor(clockSpeed.Value, 0, 250);
    }

    private static string GetTemperatureColor(float value, float min, float max)
    {
        var normalized = Math.Clamp((value - min) / (max - min), 0f, 1f);
        
        var colorStops = new[]
        {
            (0.0f, 0x1A237E),   // Deep indigo (coolest - 0%)
            (0.14f, 0x283593),  // Indigo
            (0.29f, 0x1565C0),  // Deep blue
            (0.43f, 0x0277BD),  // Blue
            (0.57f, 0x00838F),  // Teal/cyan
            (0.71f, 0xE0E0E0),  // Light gray/white (neutral)
            (0.86f, 0xFFEB3B),  // Yellow
            (0.93f, 0xFF9800),  // Orange
            (1.0f, 0xD32F2F)    // Deep red (warmest - 250%)
        };

        for (int i = 0; i < colorStops.Length - 1; i++)
        {
            var (stop1, color1) = colorStops[i];
            var (stop2, color2) = colorStops[i + 1];

            if (normalized >= stop1 && normalized <= stop2)
            {
                var t = (normalized - stop1) / (stop2 - stop1);
                return InterpolateColor(color1, color2, t);
            }
        }

        return "#D32F2F";
    }

    private static string InterpolateColor(int color1, int color2, float t)
    {
        var r1 = (color1 >> 16) & 0xFF;
        var g1 = (color1 >> 8) & 0xFF;
        var b1 = color1 & 0xFF;

        var r2 = (color2 >> 16) & 0xFF;
        var g2 = (color2 >> 8) & 0xFF;
        var b2 = color2 & 0xFF;

        var r = (int)(r1 + (r2 - r1) * t);
        var g = (int)(g1 + (g2 - g1) * t);
        var b = (int)(b1 + (b2 - b1) * t);

        return $"#{r:X2}{g:X2}{b:X2}";
    }
}

