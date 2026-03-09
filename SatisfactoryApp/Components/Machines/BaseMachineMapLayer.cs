using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Components;

namespace SatisfactoryApp.Components.Machines;

public abstract class BaseMachineMapLayer : BaseMapLayer<Factory>
{
    protected override (double x, double y) GetItemPosition(Factory item, Func<float, float, (double x, double y)> getScaledPosition)
    {
        return getScaledPosition(item.X, item.Y);
    }

    protected override string GetItemShape(Factory item)
    {
        return item.IsVariablePower() ? "triangle" : "circle";
    }
}
