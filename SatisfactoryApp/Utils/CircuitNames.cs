using Denxorz.Satisfactory.Routes.Types;
namespace SatisfactoryApp.Utils;

public static class CircuitNames
{
    public static string GetFilterName(PowerCircuit circuit, int count)
    {
        return $"{GetName(circuit)} ({count})";
    }

    public static string GetName(PowerCircuit? circuit)
    {
        if (circuit is null)
        {
            return $"[unknown]";
        }

        if (circuit.Id == -1)
        {
            if (circuit.ParentCircuitId is not null)
            {
                return $"[fracking]";
            }
            return $"[unknown / fracking]";
        }

        if (circuit.ParentCircuitId is null)
        {
            return $"[{circuit.Id}] Main";
        }
        return $"[{circuit.Id}] {circuit.Name ?? ""}";
    }
}
