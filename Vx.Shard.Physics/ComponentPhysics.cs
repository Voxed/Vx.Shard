using Vx.Shard.Common;
using Vx.Shard.Core;

namespace Vx.Shard.Physics;

public class ComponentPhysics : IComponent
{
    public Vec2 Velocity = Vec2.Zero;
    public Vec2 Acceleration = Vec2.Zero;
}