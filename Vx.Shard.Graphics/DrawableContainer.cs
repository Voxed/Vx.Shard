namespace Vx.Shard.Graphics;

using Common;

public class DrawableContainer : IDrawable
{
    private readonly List<IDrawable> _children = new();
    private float _zOrder;
    public IEnumerable<IDrawable> Children => _children.AsReadOnly();

    public Vec2 Position { get; set; } = Vec2.Zero;

    public Vec2 Scaling { get; set; } = Vec2.One;
    public float Rotation { get; set; } = 0;
    public Vec2 Pivot { get; set; } = Vec2.Zero;

    Action<IDrawable>? IDrawable.OrderChangeCallback { get; set; }

    public float ZOrder
    {
        get => _zOrder;
        set
        {
            _zOrder = value;
            ((IDrawable) this).OrderChangeCallback(this);
        }
    }

    public void Accept<T>(T context, IDrawableVisitor<T> visitor)
    {
        visitor.VisitContainer(context, this);
    }

    public void AddChild(IDrawable drawable)
    {
        drawable.OrderChangeCallback = drawable1 =>
        {
            var current = _children.IndexOf(drawable1);
            if ((current + 1 < _children.Count && drawable1.ZOrder <= _children[current + 1].ZOrder) &&
                (current - 1 >= 0 && drawable1.ZOrder >= _children[current - 1].ZOrder))
            {
                return;
            }


            _children.Remove(drawable1);

            var added = false;
            for (var i = 0; i < _children.Count; i++)
                if (drawable1.ZOrder < _children[i].ZOrder)
                {
                    _children.Insert(i, drawable1);
                    added = true;
                    break;
                }

            if (!added)
                _children.Add(drawable1);
        };
        var added = false;
        for (var i = 0; i < _children.Count; i++)
            if (drawable.ZOrder < _children[i].ZOrder)
            {
                _children.Insert(i, drawable);
                added = true;
            }

        if (!added)
            _children.Add(drawable);
    }

    public void RemoveChild(IDrawable drawable)
    {
        _children.Remove(drawable);
    }
}