namespace SatisfactoryApp.Models;

public class CargoFlowSummary
{
    public string CargoType { get; set; } = string.Empty;
    public double Produced { get; set; }
    public double Consumed { get; set; }
    public double Available => Produced - Consumed;
}


