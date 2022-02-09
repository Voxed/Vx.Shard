namespace Vx.Shard.Graphics;

using Common;

public class DrawableContainer : IDrawable
{
    private readonly List<IDrawable> _children = new();
    public IEnumerable<IDrawable> Children => _children.AsReadOnly();

    public Vec2 Position { get; set; } = Vec2.Zero;

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