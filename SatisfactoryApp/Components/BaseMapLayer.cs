using System.Globalization;

namespace SatisfactoryApp.Components.Factories;

public abstract class BaseMapLayer<T> : IMapLayer<T>
{
    public virtual string Class { get; } = "map-svg-overlay-filtered";
    public bool IsVisible { get; set; } = true;

    public List<string> CreateSvgElements(Func<float, float, (double x, double y)> getScaledPosition)
    {
        return [.. GetItems().Select(item =>
        {
            var (x, y) = GetItemPosition(item, getScaledPosition);
            var fillColor = GetItemColor(item);
            var borderColor = GetItemBorderColor(item);
            var shape = GetItemShape(item);
            var strokeWidth = GetItemStrokeWidth(item);

            if (shape == "triangle")
            {
                const double size = 0.15;
                var topX = x;
                var topY = y - size / 2;
                var leftX = x - size / 2;
                var leftY = y + size / 2;
                var rightX = x + size / 2;
                var rightY = y + size / 2;

                return string.Create(CultureInfo.InvariantCulture,
                    $"<polygon points=\"{topX:F2},{topY:F2} {leftX:F2},{leftY:F2} {rightX:F2},{rightY:F2}\" fill=\"{fillColor}\" stroke=\"{borderColor}\" stroke-width=\"{strokeWidth:F2}\" />");
            }

            const double radius = 0.05;
            return string.Create(CultureInfo.InvariantCulture,
                $"<circle cx=\"{x:F2}\" cy=\"{y:F2}\" r=\"{radius:F2}\" fill=\"{fillColor}\" stroke=\"{borderColor}\" stroke-width=\"{strokeWidth:F2}\" />");
        })];
    }

    protected abstract List<T> GetItems();
    protected abstract (double x, double y) GetItemPosition(T item, Func<float, float, (double x, double y)> getScaledPosition);
    protected abstract string GetItemColor(T item);
    protected virtual string GetItemBorderColor(T item) => "#000000";
    protected virtual string GetItemShape(T item) => "circle";
    protected virtual float GetItemStrokeWidth(T item) => 0.02f;
    public virtual List<(float X, float Y, T Item)> ItemsWithTooltip() => [];
}
