namespace Vx.Shard.Core;

public class ComponentStoreListenerBuilder
{
    private class ConcreteComponentStoreListener : IComponentStoreListener
    {

        private readonly World world;
        private Dictionary<Type, List<(Callback, Callback)>> callbacks;
        private ComponentStore store;

        public ConcreteComponentStoreListener(World world, ComponentStore store, Dictionary<Type, List<(Callback, Callback)>> callbacks)
        {
            this.world = world;
            this.callbacks = callbacks;
            this.store = store;
        }

        public void onCreation<T>(int entityId) where T : IComponent
        {
            if (callbacks.ContainsKey(typeof(T))) {
                callbacks[typeof(T)].ForEach(cb =>
                {
                    cb.Item1(world, new Entity(entityId, store));
                });
            }
        }

        public void onDestruction<T>(int entityId) where T : IComponent
        {
            if (callbacks.ContainsKey(typeof(T))) {
                callbacks[typeof(T)].ForEach(cb =>
                {
                    cb.Item2(world, new Entity(entityId, store));
                });
            }
        }
    }
    public delegate void Callback(World world, Entity entity);

    private readonly Dictionary<Type, List<(Callback, Callback)>> callbacks
        = new Dictionary<Type, List<(Callback, Callback)>>();

    public ComponentStoreListenerBuilder AddCallback<T>((Callback, Callback) callback) where T : IComponent
    {
        if (!callbacks.ContainsKey(typeof(T)))
        {
            callbacks.Add(typeof(T), new List<(Callback, Callback)>());
        }

        callbacks[typeof(T)].Add(callback);

        return this;
    }

    public IComponentStoreListener Build(World world, ComponentStore store)
    {
        return new ConcreteComponentStoreListener(world, store, callbacks);
    }
}