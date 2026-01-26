namespace SatisfactoryApp.Services;

public class LoadedFileStore
{
    private string? _loadedFileName;

    public string? LoadedFileName => _loadedFileName;

    public event Action? LoadedFileNameChanged;

    public void SetLoadedFileName(string? loadedFileName)
    {
        _loadedFileName = loadedFileName;
        LoadedFileNameChanged?.Invoke();
    }
}
