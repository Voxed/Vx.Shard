using Vx.Shard.Common;
using Vx.Shard.Core;
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
        foreach (var entity in world.GetEntitiesWith<ComponentPlayerController>().With<ComponentVelocity>())
        {
            var ctrl = entity.GetComponent<ComponentPlayerController>()!;
            
            var newVelocity = ((world.GetSingletonComponent<ComponentMouse>()!.Position -
                               entity.GetComponent<ComponentPosition>()!.Position) / 8.0f) * 50f;

            if (newVelocity.Distance() > ctrl.MaxVelocity)
            {
                newVelocity = newVelocity / newVelocity.Distance() * ctrl.MaxVelocity;
            }
            
            entity.GetComponent<ComponentVelocity>()!.Velocity = newVelocity;
        }
    }
}