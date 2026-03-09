using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class SloopsMachineMapLayer(MachineStore MachineStore) : BaseMachineMapLayer
{
    public override string Class { get; } = "map-svg-overlay-hassloop map-svg-overlay-filtered";

    protected override List<Factory> GetItems()
    {
        return [..
            MachineStore.Factories
             .Where(f => f.ClockSpeed.HasValue)
             .Where(f => f.HasSloop)
         ];
    }
    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForClockSpeed(factory.ClockSpeed);
    }

    protected override string GetItemBorderColor(Factory factory) => "#7B1FA2";
    protected override float GetItemStrokeWidth(Factory factory) => 0.06f;
}
