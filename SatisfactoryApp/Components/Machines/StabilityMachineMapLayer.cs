using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class StabilityMachineMapLayer(MachineStore MachineStore) : BaseMachineMapLayer
{
    protected override List<Factory> GetItems()
    {
        return [.. MachineStore.Factories.Where(f => f.PercentageProducing.HasValue)];
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForStability(factory.PercentageProducing);
    }
}
