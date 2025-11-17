namespace SatisfactoryApp.Models;

public class Station
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public string? Type { get; set; }
    public bool IsUnload { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public List<string>? CargoTypes { get; set; }
    public List<CargoFlow>? CargoFlows { get; set; }
    public List<Transporter>? Transporters { get; set; }
}

public class CargoFlow
{
    public string Type { get; set; } = string.Empty;
    public bool IsUnload { get; set; }
    public double? FlowPerMinute { get; set; }
    public bool IsExact { get; set; }
}

public class Transporter
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string From { get; set; } = string.Empty;
    public string? To { get; set; }
    public List<string>? OtherStops { get; set; }
}

public class Uploader
{
    public string? Id { get; set; }
    public double? X { get; set; }
    public double? Y { get; set; }
    public List<string>? CargoTypes { get; set; }
}

