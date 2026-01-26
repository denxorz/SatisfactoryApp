using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Components.Factories;

public abstract class BaseFilteredFactoryMapLayer : BaseFactoryMapLayer
{
    public override string Class { get; } = "";
    public override List<(float X, float Y, Factory Item)> ItemsWithTooltip()
    {
        return
            [..
                GetItems()
                .Select(f => (f.X, f.Y, Item: f))
            ];
    }
}
