// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Graphics;

using Core;

public class SystemGraphics : ISystem
{
    private delegate void RegisterDrawableCallback(ComponentStoreListenerBuilder componentStoreListenerBuilder);

    private readonly List<RegisterDrawableCallback> registerDrawableCallbacks = new();

    public void RegisterDrawable<T>() where T : IDrawableComponent, IComponent
    {
        registerDrawableCallbacks.Add(componentStoreListenerBuilder =>
        {
            componentStoreListenerBuilder.AddCallback<T>((
                (world, entity) =>
                {
                    world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                    {
                        e.GetComponent<ComponentGraphicsScene>()!.Root.AddChild(entity.GetComponent<T>()!
                            .GetDrawable());
                    });
                },
                (world, entity) =>
                {
                    world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                    {
                        e.GetComponent<ComponentGraphicsScene>()!.Root.RemoveChild(entity.GetComponent<T>()!
                            .GetDrawable());
                    });
                }
            ));
        });
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        registerDrawableCallbacks.ForEach(cb => cb(componentStoreListenerBuilder));
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentGraphicsScene());
    }
}