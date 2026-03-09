using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class ClockSpeedFilteredMachineMapLayer(MachineStore MachineStore) : BaseFilteredMachineMapLayer
{
    protected override List<Factory> GetItems()
    {
        return [.. MachineStore.FilteredFactories.Where(f => f.ClockSpeed.HasValue)];
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForClockSpeed(factory.ClockSpeed);
    }
}
