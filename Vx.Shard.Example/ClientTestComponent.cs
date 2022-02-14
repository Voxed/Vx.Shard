namespace Vx.Shard.Example;

using Core;
using Graphics;

public record ClientTestComponent : IComponent, IDrawableComponent
{
    public IDrawable Drawable { get; init; }
    public List<IDrawable> Ch { get; init; }
}