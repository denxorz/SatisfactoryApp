using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Factories;
using SatisfactoryApp.Utils;

namespace SatisfactoryApp.Components.Factories;

public class ClockSpeedFilteredFactoryMapLayer(FactoryStore FactoryStore) : BaseFilteredMapLayer
{
    protected override List<Factory> GetFactories()
    {
        return [.. FactoryStore.FilteredFactories.Where(f => f.ClockSpeed.HasValue)];
    }

    protected override string GetFactoryColor(Factory factory)
    {
        return FactoryColors.GetFactoryColorForClockSpeed(factory.ClockSpeed);
    }
}