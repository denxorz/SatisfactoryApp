using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class StabilityFilteredFactoryMapLayer(FactoryStore FactoryStore) : BaseFilteredMapLayer
{
    protected override List<Factory> GetFactories()
    {
        return [.. FactoryStore.FilteredFactories.Where(f => f.PercentageProducing.HasValue)];
    }

    protected override string GetFactoryColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForStability(factory.PercentageProducing);
    }
}
