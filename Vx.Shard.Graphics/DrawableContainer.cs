namespace Vx.Shard.Graphics;

using Common;

public class DrawableContainer : IDrawable
{
    private readonly List<IDrawable> _children = new();
    public IEnumerable<IDrawable> Children => _children;

    public Vec2 Position { get; set; } = Vec2.Zero;

    public Vec2 Scaling { get; set; } = Vec2.One;
    public float Rotation { get; set; } = 0;
    public Vec2 Pivot { get; set; } = Vec2.Zero;

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
        visitor.VisitContainer(context, this);
    }

    public void AddChild(IDrawable drawable)
    {
        _children.Add(drawable);
        drawable.OrderChangedListener = () => { _children.Sort((d1, d2) => d1.ZOrder - d2.ZOrder); };
        _children.Sort((d1, d2) => d1.ZOrder - d2.ZOrder);
    }

    public void RemoveChild(IDrawable drawable)
    {
        _children.Remove(drawable);
    }
}