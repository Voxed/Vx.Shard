namespace Vx.Shard.Core;

public interface ISystem
{
    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder);

    public void Initialize(World world);
}