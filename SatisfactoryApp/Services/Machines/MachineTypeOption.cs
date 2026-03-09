namespace SatisfactoryApp.Services.Machines;

public record MachineTypeOption(string Title, string Value)
{
    public override string ToString() => Title;
}
