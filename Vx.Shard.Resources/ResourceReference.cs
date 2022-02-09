namespace Vx.Shard.Resources;

public class ResourceReference
{
    public string Path { get; init; }
    public Type Type { get; init; }
    internal IResource? Resource { get; set; }
    
    public T GetResource<T>() where T : IResource
    {
        if (Resource == null)
            throw new InvalidOperationException("Resource reference used before initialization.");
        if (typeof(T) != Type)
            throw new InvalidCastException("Tried to perform invalid resource cast");
        return (T)Resource;
    }
}