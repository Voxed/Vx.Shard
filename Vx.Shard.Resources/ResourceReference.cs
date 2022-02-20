namespace Vx.Shard.Resources;

public class ResourceReference
{
    public string Path { get; init; }
    public string Type { get; init; }

    public IResource Resource
    {
        get
        {
            if (_resource == null)
                throw new NullReferenceException("Reference not initialized yet");
            return _resource;
        }
    }

    private IResource? _resource;

    public ResourceReference(string path, string type)
    {
        Path = path;
        Type = type;
    }

    internal void SetResource(IResource resource)
    {
        _resource = resource;
    }
}