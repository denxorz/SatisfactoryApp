using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Components;

namespace SatisfactoryApp.Components.Machines;

public abstract class BaseFilteredMachineMapLayer : BaseMachineMapLayer
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
