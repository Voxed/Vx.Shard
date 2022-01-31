namespace Vx.Shard.Graphics;

using System.Collections.ObjectModel;
using Common;

public class DrawableContainer : IDrawable
{
    public Vec2 Position = Vec2.Zero;

    private readonly List<IDrawable> children = new();
    public IEnumerable<IDrawable> Children => children.AsReadOnly();

    public void Accept<T>(T context, IDrawableVisitor<T> visitor)
    {
        visitor.VisitContainer(context, this);
    }

    public void AddChild(IDrawable drawable)
    {
        children.Add(drawable);
    }

    public void RemoveChild(IDrawable drawable)
    {
        children.Remove(drawable);
    }
}