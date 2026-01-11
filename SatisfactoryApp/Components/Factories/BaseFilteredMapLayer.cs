namespace SatisfactoryApp.Components.Factories;

public abstract class BaseFilteredMapLayer : BaseMapLayer
{
    public override string Class { get; } = "";

    public override List<T> ItemsWithTooltip<T>()
    {
        return [.. GetFactories().Cast<T>()];
    }
}
