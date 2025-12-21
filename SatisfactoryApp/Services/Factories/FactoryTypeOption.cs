namespace SatisfactoryApp.Services.Factories;

public record FactoryTypeOption(string Title, string Value)
{
    public override string ToString() => Title;
}

