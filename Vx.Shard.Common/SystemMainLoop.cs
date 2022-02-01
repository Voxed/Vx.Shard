// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Common;

using Core;

/// <summary>
/// The MainLoop system. It invokes a main loop in initialization and transmits MessageUpdate continuously while the
/// singleton component ComponentMainLoop has Running = true.
/// </summary>
public class SystemMainLoop : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        messageRegistry.Register<MessageUpdate>();
        componentRegistry.Register<ComponentMainLoop>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentMainLoop
        {
            StartTime = DateTime.Now
        });
        world.GetEntitiesWith<ComponentMainLoop>().ToList().ForEach(e =>
        {
            var lastTick = DateTime.Now;
            while (e.GetComponent<ComponentMainLoop>()!.Running)
            {
                var delta = DateTime.Now - lastTick;
                lastTick = DateTime.Now;
                world.Send(new MessageUpdate
                {
                    Delta = delta
                });
            }
        });
    }
}