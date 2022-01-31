namespace Vx.Shard.Graphics;

using Vx.Shard.Core;

public class SystemGraphics : ISystem
{
    private delegate void RegisterDrawableCallback(ComponentStoreListenerBuilder componentStoreListenerBuilder);
    private readonly List<RegisterDrawableCallback> registerDrawableCallbacks = new List<RegisterDrawableCallback>();

    public void RegisterDrawable<T>() where T : IDrawableComponent, IComponent
    {
        registerDrawableCallbacks.Add(cslb =>
        {
            cslb.AddCallback<T>(((World world, Entity entity) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.root.AddChild(entity.GetComponent<T>()!.GetDrawable());
                });
            }, (World world, Entity entity) =>
            {
                world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentGraphicsScene>()!.root.RemoveChild(entity.GetComponent<T>()!.GetDrawable());
                });
            }
            ));
        });
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        registerDrawableCallbacks.ForEach(cb => cb(componentStoreListenerBuilder));
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentGraphicsScene());
    }
}