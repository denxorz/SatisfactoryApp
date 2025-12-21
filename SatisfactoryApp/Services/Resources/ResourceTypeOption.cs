namespace SatisfactoryApp.Services.Resources;

public record ResourceTypeOption(string Title, string Value)
{
    public override string ToString() => Title;
}
