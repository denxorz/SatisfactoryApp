using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class PowerCircuitMachineMapLayer(MachineStore MachineStore) : BaseMachineMapLayer
{
    protected override List<Factory> GetItems()
    {
        return MachineStore.Factories;
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForPowerCircuit(factory.SubPowerCircuitId);
    }
}
