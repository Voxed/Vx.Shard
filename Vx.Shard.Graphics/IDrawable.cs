using Vx.Shard.Common;

namespace Vx.Shard.Graphics;

public interface IDrawable
{
    public Vec2 Position { get; set; }

    public Vec2 Scaling { get; set; }

    public float Rotation { get; set; }
    public Vec2 Pivot { get; set; }

    public int ZOrder { get; set; }
    
    public Color Tint { get; set; }
    
    public float Opacity { get; set; }

    internal Action? OrderChangedListener { get; set; }

    void Accept<T>(T context, IDrawableVisitor<T> visitor);
}