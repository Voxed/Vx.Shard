namespace Vx.Shard.Graphics;

using Common;

public class DrawableContainer : IDrawable
{
    public Vec2 Position = Vec2.Zero;

    private readonly List<IDrawable> _children = new();
    public IEnumerable<IDrawable> Children => _children.AsReadOnly();

    public void Accept<T>(T context, IDrawableVisitor<T> visitor)
    {
        visitor.VisitContainer(context, this);
    }

    public void AddChild(IDrawable drawable)
    {
        _children.Add(drawable);
    }

    public void RemoveChild(IDrawable drawable)
    {
        _children.Remove(drawable);
    }
}