namespace Vx.Shard.Graphics;

/// <summary>
/// Convenient component for entities.
/// 
/// This is a standard drawable container component which can be used to collect multiple components' drawables into
/// one container. Useful for controlling all drawables attached to the same entity.
/// </summary>
public record ComponentStandardDrawableContainer : IDrawableComponent
{
    public readonly DrawableContainer Container = new();
    public IDrawable Drawable => Container;
}