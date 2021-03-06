using Vx.Shard.Core;

namespace Vx.Shard.Window;

public class SystemInput : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        messageRegistry
            .Register<MessageWindowClose>()
            .Register<MessageMouseMove>();
        componentRegistry.Register<ComponentMouse>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentMouse());
    }
}