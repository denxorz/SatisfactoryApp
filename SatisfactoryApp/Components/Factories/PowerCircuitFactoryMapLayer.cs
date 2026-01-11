using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class PowerCircuitFactoryMapLayer(FactoryStore FactoryStore) : BaseMapLayer
{
    protected override List<Factory> GetFactories()
    {
        return FactoryStore.Factories;
    }

    protected override string GetFactoryColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForPowerCircuit(factory.SubPowerCircuitId);
    }
}
