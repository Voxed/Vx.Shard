using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public record MessageUnloadResource : IMessage
{
    public readonly ResourceReference Reference;

    public MessageUnloadResource(ResourceReference reference)
    {
        Reference = reference;
    }
}