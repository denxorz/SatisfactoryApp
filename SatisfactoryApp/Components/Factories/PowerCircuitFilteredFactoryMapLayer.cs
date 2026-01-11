using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class PowerCircuitFilteredFactoryMapLayer(FactoryStore FactoryStore) : BaseFilteredMapLayer
{
    protected override List<Factory> GetFactories()
    {
        return FactoryStore.FilteredFactories;
    }

    protected override string GetFactoryColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForPowerCircuit(factory.SubPowerCircuitId);
    }
}
