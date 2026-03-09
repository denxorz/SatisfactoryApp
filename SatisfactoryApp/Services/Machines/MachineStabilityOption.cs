namespace SatisfactoryApp.Services.Machines;

public record MachineStabilityOption(string Title, string Value)
{
    public override string ToString() => Title;
}
