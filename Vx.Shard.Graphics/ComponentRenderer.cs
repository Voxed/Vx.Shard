using Vx.Shard.Core;

namespace Vx.Shard.Graphics;

public class ComponentRenderer : IComponent
{
    public DrawableContainer DrawableContainer = new DrawableContainer();

    public ComponentRenderer(DrawableContainer drawableContainer)
    {
        DrawableContainer = drawableContainer;
    }

    public ComponentRenderer()
    {
        
    }
}