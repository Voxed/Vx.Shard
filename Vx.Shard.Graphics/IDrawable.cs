using Vx.Shard.Common;

namespace Vx.Shard.Graphics;

public interface IDrawable
{
    public Vec2 Position { get; set; }
    void Accept<T>(T context, IDrawableVisitor<T> visitor);
}