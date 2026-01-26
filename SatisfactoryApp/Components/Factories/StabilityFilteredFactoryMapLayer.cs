using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class StabilityFilteredFactoryMapLayer(FactoryStore FactoryStore) : BaseFilteredFactoryMapLayer
{
    protected override List<Factory> GetItems()
    {
        return [.. FactoryStore.FilteredFactories.Where(f => f.PercentageProducing.HasValue)];
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForStability(factory.PercentageProducing);
    }
}
