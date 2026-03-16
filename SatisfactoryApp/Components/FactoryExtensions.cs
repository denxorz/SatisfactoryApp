using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Components;

public static class FactoryExtensions
{
    private static readonly string[] variablePowerTypes =
    [
        "Converter",
        "HadronCollider",
        "QuantumEncoder",
        "TrainStation",
        "ResourceSink",
        "GeneratorGeoThermal"
    ];

    public static bool IsVariablePower(this Factory f) => variablePowerTypes.Contains(f.Type);
    public static bool IsPowerStorage(this Factory f) => f.Type == "PowerStorage";
    public static bool IsPowerProducerStorage(this Factory f) => f.Type.StartsWith("Generator");
    public static bool IsStable(this Factory f) => f.PercentageProducing == 100;

    public static string GetStabilityTooltip(this Factory f)
    {
        return f.PercentageProducing switch
        {
            null => "Off or unknown",
            100 => "Stable (100%)",
            _ => $"Running at {f.PercentageProducing:F1}%"
        };
    }

    public static string GetFactoryStability(this Factory f)
    {
        return f.PercentageProducing switch
        {
            null => "Unknown",
            100 => "Stable",
            >= 95 and < 100 => "Almost Stable",
            >= 1 and < 95 => "Unstable",
            _ => "Off"
        };
    }
}
