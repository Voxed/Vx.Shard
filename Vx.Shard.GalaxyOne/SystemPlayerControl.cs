using Vx.Shard.Collision;
using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Physics;
using Vx.Shard.Window;

namespace Vx.Shard.GalaxyOne;

public class SystemPlayerControl : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPlayerController>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    {
    }

    private void Update(World world, MessageUpdate msg)
    {
        // After a lot of experimenting, this is the simple control algorithm I ended up with. I'm not especially proud
        // of my physics display.
        foreach (var entity in world.GetEntitiesWith<ComponentPlayerController>().With<ComponentPhysics>())
        {
            var physics = entity.GetComponent<ComponentPhysics>()!;
            var ctrl = entity.GetComponent<ComponentPlayerController>()!;

            var targetVelocity = (world.GetSingletonComponent<ComponentMouse>()!.Position -
                                  entity.GetComponent<ComponentPosition>()!.Position) * 5;

            physics.Acceleration = (targetVelocity - physics.Velocity) * 50;

            if (physics.Acceleration.Distance() > ctrl.MaxAcceleration)
                physics.Acceleration = physics.Acceleration.Normalize() * ctrl.MaxAcceleration;

            if (physics.Velocity.Distance() > ctrl.MaxVelocity)
            {
                physics.Velocity = physics.Velocity / physics.Velocity.Distance() * ctrl.MaxVelocity;
            }
        }
    }
}