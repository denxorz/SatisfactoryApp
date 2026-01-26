using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class StabilityFactoryMapLayer(FactoryStore FactoryStore) : BaseFactoryMapLayer
{
    protected override List<Factory> GetItems()
    {
        return [.. FactoryStore.Factories.Where(f => f.PercentageProducing.HasValue)];
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForStability(factory.PercentageProducing);
    }
}
