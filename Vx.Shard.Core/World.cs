namespace Vx.Shard.Core;

public class World
{
    private readonly List<ISystem> systems;
    private readonly MessageBus messageBus = new MessageBus();
    private readonly ComponentStore componentStore = new ComponentStore();

    internal World(List<ISystem> systems, MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        this.systems = systems;
        this.messageBus.SetListener(messageBusListenerBuilder.Build(this));
        this.componentStore.SetListener(componentStoreListenerBuilder.Build(this, componentStore));
    }

    public void Send<T>(T message) where T : IMessage
    {
        this.messageBus.send(message);
    }

    public Entity CreateEntity()
    {
        return new Entity(componentStore.CreateEntity(), componentStore);
    }

    public EntitySet GetEntitiesWith<T>() where T : IComponent
    {
        return new EntitySet(componentStore.GetEntitiesWith<T>(), componentStore);
    }
}