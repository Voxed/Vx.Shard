namespace Vx.Shard.Resources;

public class ResourceInitializer
{
    public string Path { get; init; }
    public string Type { get; init; }
    public IResource? Resource { get; set; }

    public ResourceInitializer(string path, string type)
    {
        Path = path;
        Type = type;
    }
}