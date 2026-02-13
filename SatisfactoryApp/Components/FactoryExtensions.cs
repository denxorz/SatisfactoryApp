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

    extension(Factory f)
    {
        public bool IsVariablePower() => variablePowerTypes.Contains(f.Type);
        public bool IsPowerStorage() => f.Type == "PowerStorage";
        public bool IsPowerProducerStorage() => f.Type.StartsWith("Generator");
        public bool IsStable() => f.PercentageProducing == 100;

        public string GetStabilityTooltip()
        {
            return f.PercentageProducing switch
            {
                null => "Off or unknown",
                100 => "Stable (100%)",
                _ => $"Running at {f.PercentageProducing:F1}%"
            };
        }

        public string GetFactoryStability()
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
}
