using Vx.Shard.Common;

namespace Vx.Shard.Graphics;

public interface IDrawable
{
    public Vec2 Position { get; set; }

    public Vec2 Scaling { get; set; }

    public float Rotation { get; set; }
    public Vec2 Pivot { get; set; }

    internal Action<IDrawable> OrderChangeCallback { get; set; }

    public float ZOrder { get; set; }
    
    public Color Tint { get; set; }
    
    public float Opacity { get; set; }

    void Accept<T>(T context, IDrawableVisitor<T> visitor);
}