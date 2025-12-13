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
            return $"None";
        }

        if (circuit.Id == -1)
        {
            if (circuit.ParentCircuitId is not null)
            {
                return $"Fracking";
            }
            return $"None/Fracking";
        }

        return string.Join(" ",
            new List<string?>()
            {
                $"[{circuit.Id.ToString()}]",
                circuit.ParentCircuitId is null ? "Main" : null,                
                circuit.Name
            }
            .Where(testc => testc is not null));
    }
}

