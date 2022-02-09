using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public record MessageUnloadResource : IMessage
{
    public string Path { get; init; }
}