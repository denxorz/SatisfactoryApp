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

    public static bool IsVariablePower(this Factory f)
    {
        return variablePowerTypes.Contains(f.Type);
    }

    public static bool IsPowerStorage(this Factory f)
    {
        return f.Type == "PowerStorage";
    }

    public static bool IsPowerProducerStorage(this Factory f)
    {
        return f.Type.StartsWith("Generator");
    }
}
