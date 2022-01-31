namespace Vx.Shard.Example;

using Vx.Shard.Core;

public record UpdateMessage : IMessage
{
    public float Delta { get; init; }
}