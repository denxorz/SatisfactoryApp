namespace SatisfactoryApp.Services.Machines;

public record PowerCircuitOption(string Title, string Value)
{
    public override string ToString() => Title;
}
