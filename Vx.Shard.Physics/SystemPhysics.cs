using Vx.Shard.Common;
using Vx.Shard.Core;

namespace Vx.Shard.Physics;

public class SystemPhysics : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPhysics>();
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
        // Apply velocity to position component.
        foreach (var entity in world.GetEntitiesWith<ComponentPhysics>().With<ComponentPosition>())
        {
            var position = entity.GetComponent<ComponentPosition>()!;
            var velocity = entity.GetComponent<ComponentPhysics>()!;
            position.Position += velocity.Velocity * (float) msg.Delta.TotalSeconds;
            velocity.Velocity *= (float) Math.Pow(0.05, msg.Delta.TotalSeconds); // Damping
            velocity.Velocity += velocity.Acceleration * (float) msg.Delta.TotalSeconds; // Apply acceleration
            velocity.Acceleration = Vec2.Zero; // Set acceleration to zero
        }
    }
}