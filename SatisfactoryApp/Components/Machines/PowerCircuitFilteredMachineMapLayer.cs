using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class PowerCircuitFilteredMachineMapLayer(MachineStore MachineStore) : BaseFilteredMachineMapLayer
{
    protected override List<Factory> GetItems()
    {
        return MachineStore.FilteredFactories;
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForPowerCircuit(factory.SubPowerCircuitId);
    }
}
