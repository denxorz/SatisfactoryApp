using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Components.Factories;

public abstract class BaseMapLayer : IMapLayer
{
    public virtual string Class { get; } = "map-svg-overlay-filtered";
    public bool IsVisible { get; set; } = true;

    public List<string> CreateDotEdges(Func<float, float, string> getPosition)
    {
        return [.. GetFactories()
            .Select((factory, index) =>
            {
                var fillColor = GetFactoryColor(factory);
                var borderColor = GetFactoryBorderColor(factory);
                var shape = GetFactoryShape(factory);
                var penwidth = GetPenWidth(factory);

                return $"\nfactory_{index} [" +
                       $"shape=\"{shape}\", " +
                       $"fillcolor=\"{fillColor}\", " +
                       $"color=\"{borderColor}\", " +
                       $"penwidth={penwidth}, " +
                       $"{getPosition(factory.X, factory.Y)}" +
                       $"];";
            })
        ];
    }

    protected abstract List<Factory> GetFactories();
    protected abstract string GetFactoryColor(Factory factory);
    protected virtual string GetFactoryBorderColor(Factory factory) => "#000000";

    protected virtual string GetFactoryShape(Factory factory)
    {
        return factory.IsVariablePower() ? "triangle" : "circle";
    }

    protected virtual string GetPenWidth(Factory factory) => "1";
    public virtual List<T> ItemsWithTooltip<T>() => [];
}
