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
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        componentStoreListenerBuilder.AddSubtypeCallback<IDrawableComponent>(
            (world, entity, component) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.Root.AddChild(component.Drawable);
                });
            },
            (world, entity, component) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.Root.RemoveChild(component.Drawable);
                });
            }
        );
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentGraphicsScene());
        var component = world.GetSingletonComponent<ComponentGraphicsScene>()!;
        component.Root.Scaling = new Vec2(0.4F, 0.2F);
        component.Root.Rotation = 3.14f/2;
        component.Root.Position = new Vec2(200, 200);
    }
}