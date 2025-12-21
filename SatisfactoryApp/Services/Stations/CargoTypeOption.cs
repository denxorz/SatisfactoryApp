namespace SatisfactoryApp.Services.Stations;

public record CargoTypeOption(string Title, string Value)
{
    public override string ToString() => Title;
}
