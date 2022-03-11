using Vx.Shard.Common;

namespace Vx.Shard.Graphics;

using Core;

public record ComponentGraphicsScene : IComponent
{
    public DrawableContainer Root { get; init; } = new();
    public Vec2 Size = Vec2.Zero;
    
    // My one true love, cornflowerblue <3
    public Color ClearColor = new Color(100, 149, 237);
}