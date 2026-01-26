using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class PowerCircuitFilteredFactoryMapLayer(FactoryStore FactoryStore) : BaseFilteredFactoryMapLayer
{
    protected override List<Factory> GetItems()
    {
        return FactoryStore.FilteredFactories;
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForPowerCircuit(factory.SubPowerCircuitId);
    }
}
