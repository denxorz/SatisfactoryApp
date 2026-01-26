using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Components.Factories;

public abstract class BaseFactoryMapLayer : BaseMapLayer<Factory>
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
