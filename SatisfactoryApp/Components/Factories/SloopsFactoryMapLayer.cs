using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class SloopsFactoryMapLayer(FactoryStore FactoryStore) : BaseMapLayer
{
    public override string Class { get; } = "map-svg-overlay-hassloop map-svg-overlay-filtered";

    protected override List<Factory> GetFactories()
    {
        return [..
            FactoryStore.Factories
             .Where(f => f.ClockSpeed.HasValue)
             .Where(f => f.HasSloop)
         ];
    }
    protected override string GetFactoryColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForClockSpeed(factory.ClockSpeed);
    }

    protected override string GetFactoryBorderColor(Factory factory) => "#7B1FA2";
    protected override string GetPenWidth(Factory factory) => "3";
}
