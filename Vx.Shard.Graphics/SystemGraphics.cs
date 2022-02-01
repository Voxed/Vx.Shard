// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Graphics;

using Core;

public class SystemGraphics : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentGraphicsScene>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        componentStoreListenerBuilder.AddSubtypeCallback<IDrawableComponent>(
            (world, entity, component) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.Root.AddChild(component.GetDrawable());
                });
            },
            (world, entity, component) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.Root.RemoveChild(component.GetDrawable());
                });
            }
        );
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentGraphicsScene());
    }
}