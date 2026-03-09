using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Machines;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Machines;

public class StabilityFilteredMachineMapLayer(MachineStore MachineStore) : BaseFilteredMachineMapLayer
{
    protected override List<Factory> GetItems()
    {
        return [.. MachineStore.FilteredFactories.Where(f => f.PercentageProducing.HasValue)];
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForStability(factory.PercentageProducing);
    }
}
