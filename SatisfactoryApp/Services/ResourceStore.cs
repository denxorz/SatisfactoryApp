using Denxorz.Satisfactory.Routes.Types;

namespace SatisfactoryApp.Services;

public class ResourceStore
{
    private readonly List<Resource> _resources = [];
    private int _updateCounter = 0;

    public List<Resource> Resources => _resources;
    public int UpdateCounter => _updateCounter;

    public event Action? ResourcesChanged;

    public void Set(List<Resource> resources)
    {
        _resources.Clear();
        _resources.AddRange(resources);

        _updateCounter++;

        ResourcesChanged?.Invoke();
    }
}
