using Vx.Shard.Common;
using Vx.Shard.Core;

namespace Vx.Shard.SpaceDemo;

public class ComponentPosition : IComponent
{
    public Vec2 Position = Vec2.Zero;

    public ComponentPosition(Vec2? position = null)
    {
        if (position != null)
            Position = position;
    }
}