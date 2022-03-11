// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using Vx.Shard.Common;

namespace Vx.Shard.Graphics;

using Core;

public class SystemGraphics : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentGraphicsScene>();
        componentRegistry.Register<ComponentRenderer>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        componentStoreListenerBuilder.AddCallback<ComponentRenderer>(
            (world, entity, component) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.Root.AddChild(component.DrawableContainer);
                });
            },
            (world, entity, component) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.Root.RemoveChild(component.DrawableContainer);
                });
            }
        );
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentGraphicsScene());
        var component = world.GetSingletonComponent<ComponentGraphicsScene>()!;
    }
}