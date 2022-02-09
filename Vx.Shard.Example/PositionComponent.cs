namespace Vx.Shard.Example;

using Core;

public record PositionComponent : IComponent
{
    public float X { get; set; }
    public float Y { get; set; }
    public int Index { get; init; }

}