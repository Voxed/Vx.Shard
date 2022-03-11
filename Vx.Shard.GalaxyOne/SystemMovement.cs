using Vx.Shard.Common;
using Vx.Shard.Core;

namespace Vx.Shard.GalaxyOne;

public class SystemMovement : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<Component2DVelocity>();
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
        foreach (var entity in world.GetEntitiesWith<Component2DVelocity>().With<ComponentPosition>())
        {
            var position = entity.GetComponent<ComponentPosition>()!;
            var velocity = entity.GetComponent<Component2DVelocity>()!;
            position.Position += velocity.Velocity;
            velocity.Velocity *= 0.9f;
        }
    }
}