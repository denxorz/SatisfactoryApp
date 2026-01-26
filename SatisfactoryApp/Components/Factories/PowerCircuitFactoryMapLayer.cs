using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class PowerCircuitFactoryMapLayer(FactoryStore FactoryStore) : BaseFactoryMapLayer
{
    protected override List<Factory> GetItems()
    {
        return FactoryStore.Factories;
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForPowerCircuit(factory.SubPowerCircuitId);
    }
}
