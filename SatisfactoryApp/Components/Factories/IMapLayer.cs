namespace SatisfactoryApp.Components.Factories
{
    public interface IMapLayer
    {
        string Class { get; }
        bool IsVisible { get; set; }

        List<string> CreateDotEdges(Func<float, float, string> getPosition);
        List<T> ItemsWithTooltip<T>();
    }
}