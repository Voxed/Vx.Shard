using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public record MessageLoadResource : IMessage
{
    public ResourceInitializer Initializer { get; init; }
}