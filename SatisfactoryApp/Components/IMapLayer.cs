namespace SatisfactoryApp.Components
{
    public interface IMapLayer<T>
    {
        string Class { get; }
        bool IsVisible { get; set; }

        List<string> CreateSvgElements(Func<float, float, (double x, double y)> getScaledPosition);
        List<(float X, float Y, T Item)> ItemsWithTooltip();
    }
}