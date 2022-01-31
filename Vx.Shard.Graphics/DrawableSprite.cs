namespace Vx.Shard.Graphics;

using Vx.Shard.Common;

public class DrawableSprite : IDrawable
{
    public Vec2 Position = Vec2.Zero;
    public string? TexturePath;

    public void Accept<T>(T context, IDrawableVisitor<T> visitor)
    {
        visitor.VisitSprite(context, this);
    }
}