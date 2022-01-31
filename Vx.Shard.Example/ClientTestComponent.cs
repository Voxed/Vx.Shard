namespace Vx.Shard.Example;

using Core;
using Graphics;

public record ClientTestComponent : IComponent, IDrawableComponent
{
    public readonly DrawableSprite Drawable = new();

    public IDrawable GetDrawable()
    {
        Drawable.TexturePath = "test.png";
        return Drawable;
    }
}