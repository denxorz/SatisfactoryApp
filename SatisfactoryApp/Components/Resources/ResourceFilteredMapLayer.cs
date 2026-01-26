using Denxorz.Satisfactory.Routes.Types;
using SatisfactoryApp.Services.Resources;

namespace SatisfactoryApp.Components.Resources;

public class ResourceFilteredMapLayer(ResourceStore ResourceStore) : ResourceMapLayer(ResourceStore)
{
    public override string Class { get; } = "";

    public override List<(float X, float Y, Resource Item)> ItemsWithTooltip()
    {
        return
        [..
            GetItems()
            .Select(r => (r.X, r.Y, Item: r))
        ];
    }

    protected override List<Resource> GetItems()
    {
        return ResourceStore.FilteredResources;
    }
}
