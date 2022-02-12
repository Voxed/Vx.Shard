using Vx.Shard.Common;

namespace Vx.Shard.Graphics;

public interface IDrawable
{
    public Vec2 Position { get; set; }
    
    public Vec2 Scaling { get; set; }
    
    public float Rotation { get; set; }
    public Vec2 Pivot { get; set; }
    
    void Accept<T>(T context, IDrawableVisitor<T> visitor);
}