using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;

namespace Vx.Shard.GalaxyOne;

public class ComponentResourcesShip1Renderer : IComponent
{
    public readonly ResourceReference Texture = new ResourceReference("assets/textures/ship1.png", "texture");
    public readonly DrawableSprite Sprite;

    public ComponentResourcesShip1Renderer()
    {
        Sprite = new DrawableSprite(Texture)
        {
            Scaling = new Vec2(2.0f, 2.0f),
            ScaleQuality = ScaleQuality.Nearest
        };
    }
}