using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class SloopsFilteredMachineMapLayer(MachineStore MachineStore) : BaseFilteredMachineMapLayer
{
    public override string Class { get; } = "map-svg-overlay-hassloop";

    protected override List<Factory> GetItems()
    {
        return [..
            MachineStore.FilteredFactories
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
