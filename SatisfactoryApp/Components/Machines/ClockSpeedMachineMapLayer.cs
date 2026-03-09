using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class ClockSpeedMachineMapLayer(MachineStore MachineStore) : BaseMachineMapLayer
{
    protected override List<Factory> GetItems()
    {
        return [.. MachineStore.Factories.Where(f => f.ClockSpeed.HasValue)];
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForClockSpeed(factory.ClockSpeed);
    }
}
