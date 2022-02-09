using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public record MessageUnloadResource : IMessage
{
    public Type Type { get; init; }
    public IResource Resource { get; init; }
    
    public string Path { get; init; }
}