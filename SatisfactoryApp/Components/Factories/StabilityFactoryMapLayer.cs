using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class StabilityFactoryMapLayer(FactoryStore FactoryStore) : BaseMapLayer
{
    protected override List<Factory> GetFactories()
    {
        return [.. FactoryStore.Factories.Where(f => f.PercentageProducing.HasValue)];
    }

    protected override string GetFactoryColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForStability(factory.PercentageProducing);
    }
}
