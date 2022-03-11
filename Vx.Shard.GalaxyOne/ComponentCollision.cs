using Vx.Shard.Core;

namespace Vx.Shard.GalaxyOne;

public class ComponentCollision : IComponent
{
    public float Radius;

    public ComponentCollision(float radius)
    {
        Radius = radius;
    }
}