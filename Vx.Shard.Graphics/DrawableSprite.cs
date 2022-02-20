using Vx.Shard.Resources;

namespace Vx.Shard.Graphics;

using Common;

public class DrawableSprite : IDrawable
{
    public ResourceReference Resource;

    public Vec2 Position { get; set; } = Vec2.Zero;
    public Vec2 Scaling { get; set; } = Vec2.One;

    public float Rotation { get; set; } = 0;
    public Vec2 Pivot { get; set; } = Vec2.Zero;

    public BlendMode BlendMode { get; set; } = BlendMode.Normal;
    public float Opacity { get; set; } = 0.0f;

    private int _zOrder = 0;
    public int ZOrder
    {
        get => _zOrder;
        set
        {
            _zOrder = value;
            ((IDrawable) this).OrderChangedListener?.Invoke();
        }
    }
    public Color Tint { get; set; } = Color.White;

    Action? IDrawable.OrderChangedListener { get; set; }

    public void Accept<T>(T context, IDrawableVisitor<T> visitor)
    {
        visitor.VisitSprite(context, this);
    }
}