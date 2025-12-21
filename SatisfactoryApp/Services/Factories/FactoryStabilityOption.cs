namespace SatisfactoryApp.Services.Factories;

public record FactoryStabilityOption(string Title, string Value)
{
    public override string ToString() => Title;
}

