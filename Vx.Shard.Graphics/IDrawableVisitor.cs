namespace Vx.Shard.Graphics;

public interface IDrawableVisitor<T>
{
    public void VisitSprite(T context, DrawableSprite sprite);
    public void VisitContainer(T context, DrawableContainer container);

}