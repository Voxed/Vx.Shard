namespace Vx.Shard.Example;

using Vx.Shard.Core;

public record PositionComponent : IComponent
{
    public float x { get; set; }
    public float y { get; set; }
}