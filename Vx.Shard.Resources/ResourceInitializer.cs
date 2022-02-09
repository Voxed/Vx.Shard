namespace Vx.Shard.Resources;

public class ResourceInitializer
{
    public string Path { get; init; }
    public Type Type { get; init; }
    public IResource? Resource { get; set; }
}