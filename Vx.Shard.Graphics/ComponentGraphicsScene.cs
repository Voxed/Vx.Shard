namespace Vx.Shard.Graphics;

using Core;

public record ComponentGraphicsScene : IComponent
{
    public DrawableContainer Root { get; init; } = new();
}