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
            return $"[no name]";
        }

        if (circuit.Id == -1)
        {
            if (circuit.ParentCircuitId is not null)
            {
                return $"[fracking]";
            }
            return $"[no name / fracking]";
        }

        return string.Join(" ",
            new List<string?>()
            {
                $"[{circuit.Id}]",
                circuit.ParentCircuitId is null ? "Main" : null,                
                circuit.Name ?? "[no name]"
            }
            .Where(testc => testc is not null));
    }
}

