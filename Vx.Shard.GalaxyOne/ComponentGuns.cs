using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;

namespace Vx.Shard.GalaxyOne;

public class ComponentGuns : IComponent
{
    public readonly ResourceReference[] GunTextures = new[]
        {new ResourceReference("assets/textures/gun1.png", "texture")};
    
    public readonly List<DrawableSprite> GunSprites = new();
}