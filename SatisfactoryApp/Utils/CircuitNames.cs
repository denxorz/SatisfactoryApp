using Denxorz.Satisfactory.Routes.Types;
namespace SatisfactoryApp.Utils;

public static class CircuitNames
{
    public static string GetFilterName(PowerCircuit circuit, int count)
    {
        return $"{GetIdAndName(circuit)} ({count})";
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
            return $"Main";
        }
        return circuit.Name ?? "[no name]";
    }

    public static string GetIdAndName(PowerCircuit? circuit)
    {
        if (circuit is null || circuit.Id == -1)
        {
            return GetName(circuit);
        }

        return $"[{circuit.Id}] {GetName(circuit)}";
    }
}
