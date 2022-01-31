namespace Vx.Shard.Graphics;

using Vx.Shard.Core;

public record ComponentGraphicsScene : IComponent
{
    public DrawableContainer root { get; set; } = new DrawableContainer();
}