namespace SatisfactoryApp.Services.Factories;

public record PowerCircuitOption(string Title, string Value)
{
    public override string ToString() => Title;
}

