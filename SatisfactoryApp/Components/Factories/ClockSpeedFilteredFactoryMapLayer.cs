using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class ClockSpeedFilteredFactoryMapLayer(FactoryStore FactoryStore) : BaseFilteredFactoryMapLayer
{
    protected override List<Factory> GetItems()
    {
        return [.. FactoryStore.FilteredFactories.Where(f => f.ClockSpeed.HasValue)];
    }

    protected override string GetItemColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForClockSpeed(factory.ClockSpeed);
    }
}