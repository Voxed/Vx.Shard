namespace Vx.Shard.Graphics;

using Common;

public class DrawableSprite : IDrawable
{
    public ResourceTexture Resource;

    public Vec2 Position { get; set; } = Vec2.Zero;
    public Vec2 Scaling { get; set; } = Vec2.One;

    public float Rotation { get; set; } = 0;
    public Vec2 Pivot { get; set; } = Vec2.Zero;

    public void Accept<T>(T context, IDrawableVisitor<T> visitor)
    {
        visitor.VisitSprite(context, this);
    }
}