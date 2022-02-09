using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public record MessageUnloadResource : IMessage
{
    public IResource Resource { get; init; }
}