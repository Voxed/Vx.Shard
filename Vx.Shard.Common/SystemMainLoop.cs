namespace Vx.Shard.Common;

using Vx.Shard.Core;

public class SystemMainLoop : ISystem
{
    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder)
    { }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentMainLoop
        {
            StartTime = DateTime.Now
        });
        world.GetEntitiesWith<ComponentMainLoop>().ToList().ForEach(e =>
        {
            DateTime lastTick = DateTime.Now;
            while (e.GetComponent<ComponentMainLoop>()!.Running)
            {
                float delta = (float)(DateTime.Now - lastTick).TotalSeconds;
                lastTick = DateTime.Now;
                world.Send(new MessageUpdate
                {
                    Delta = delta,
                });
            }
        });
    }
}