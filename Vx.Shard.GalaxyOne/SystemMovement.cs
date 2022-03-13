using Vx.Shard.Common;
using Vx.Shard.Core;

namespace Vx.Shard.GalaxyOne;

public class SystemMovement : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentVelocity>();
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
        foreach (var entity in world.GetEntitiesWith<ComponentVelocity>().With<ComponentPosition>())
        {
            var position = entity.GetComponent<ComponentPosition>()!;
            var velocity = entity.GetComponent<ComponentVelocity>()!;
            position.Position += velocity.Velocity * (float) msg.Delta.TotalSeconds;
            velocity.Velocity -= velocity.Velocity * (float) msg.Delta.TotalSeconds * 2.0f;
        }
    }
}