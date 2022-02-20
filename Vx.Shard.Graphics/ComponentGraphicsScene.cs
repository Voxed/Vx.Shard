using Vx.Shard.Common;

namespace Vx.Shard.Graphics;

using Core;

public record ComponentGraphicsScene : IComponent
{
    public DrawableContainer Root { get; init; } = new();
    public Vec2 Size = Vec2.Zero;
}