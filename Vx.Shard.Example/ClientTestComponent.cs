namespace Vx.Shard.Example;

using Vx.Shard.Core;
using Vx.Shard.Graphics;

public record ClientTestComponent : IComponent, IDrawableComponent
{
    public string wow { get; set; } = "";
    public DrawableSprite drawable = new DrawableSprite();

    public IDrawable GetDrawable()
    {
        drawable.TexturePath = "test.png";
        return drawable;
    }
}