using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Components.Factories;
using SatisfactoryApp.Services.Resources;

namespace SatisfactoryApp.Components.Resources;

public class ResourceMapLayer(ResourceStore ResourceStore) : BaseMapLayer<Resource>
{
    protected override (double x, double y) GetItemPosition(Resource item, Func<float, float, (double x, double y)> getScaledPosition)
    {
        return getScaledPosition(item.X, item.Y);
    }

    protected override List<Resource> GetItems()
    {
        return ResourceStore.Resources;
    }

    protected override string GetItemColor(Resource item)
    {
        return Utils.Resources.GetResourceColor(item.Type);
    }

    protected override string GetItemBorderColor(Resource item)
    {
        if (Math.Abs(item.Max - item.Flow) < 0.1)
        {
            return "#FF0000";
        }
        else if (item.Flow > 0)
        {
            return "#FFA500";
        }
        else
        {
            return "#000000";
        }
    }

    protected override float GetItemStrokeWidth(Resource item)
    {
        if (Math.Abs(item.Max - item.Flow) < 0.1)
        {
            return 0.03f;
        }
        else if (item.Flow > 0)
        {
            return 0.02f;
        }
        else
        {
            return 0.01f;
        }
    }
}
