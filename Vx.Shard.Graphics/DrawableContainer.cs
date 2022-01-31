namespace Vx.Shard.Graphics;

using System.Collections.ObjectModel;
using Vx.Shard.Common;

public class DrawableContainer : IDrawable
{
    public Vec2 Position = Vec2.Zero;

    private readonly List<IDrawable> children = new List<IDrawable>();
    public ReadOnlyCollection<IDrawable> Children { get { return children.AsReadOnly(); } }

    public void Accept<T>(T context, IDrawableVisitor<T> visitor)
    {
        visitor.VisitContainer(context, this);
    }

    public void AddChild(IDrawable drawable) {
        children.Add(drawable);
    }

    public void RemoveChild(IDrawable drawable) {
        children.Remove(drawable);
    }
}