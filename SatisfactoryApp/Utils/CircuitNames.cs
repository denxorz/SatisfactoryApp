namespace SatisfactoryApp.Utils;

public static class CircuitNames
{
    public static string GetMainCircuitName(int? circuitId)
    {
        if (circuitId == null) return "-";
        if (circuitId == -1) return "None/Fracking";
        return circuitId.Value.ToString();
    }

    public static string GetSubCircuitName(int? circuitId)
    {
        if (circuitId == null) return "-";
        if (circuitId == -1) return "Fracking";
        return circuitId.Value.ToString();
    }

    public static string GetMainCircuitFilterName(int circuitId, int count)
    {
        if (circuitId == -1) return $"None/Fracking ({count})";
        return $"Main {circuitId} ({count})";
    }

    public static string GetSubCircuitFilterName(int circuitId, int count)
    {
        if (circuitId == -1) return $"Fracking ({count})";
        return $"Sub {circuitId} ({count})";
    }
}

