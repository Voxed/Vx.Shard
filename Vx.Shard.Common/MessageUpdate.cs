namespace Vx.Shard.Common;

using Vx.Shard.Core;

public record MessageUpdate : IMessage
{
    public float Delta { get; init; }
}