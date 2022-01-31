namespace Vx.Shard.Core;

public interface IComponentStoreListener
{

    public void onCreation<T>(int entityId) where T : IComponent;
    public void onDestruction<T>(int entityId) where T : IComponent;

}

public class ComponentStore
{
    private readonly Dictionary<Type, Dictionary<int, IComponent>> components = new Dictionary<Type, Dictionary<int, IComponent>>();

    private int nextEntityId = 0;
    private IComponentStoreListener? listener;

    internal void SetListener(IComponentStoreListener listener)
    {
        this.listener = listener;
    }

    internal int CreateEntity()
    {
        return nextEntityId++;
    }

    internal void AddComponent<T>(int entityId, T component) where T : IComponent
    {
        if (!components.ContainsKey(typeof(T)))
            components.Add(typeof(T), new Dictionary<int, IComponent>());
        Dictionary<int, IComponent> entityComponentStore = components[typeof(T)];
        if (!entityComponentStore.ContainsKey(entityId))
        {
            entityComponentStore.Add(entityId, component);
            if (listener != null)
                listener.onCreation<T>(entityId);
        }
    }

    internal void RemoveComponent<T>(int entityId) where T : IComponent
    {
        if (components.ContainsKey(typeof(T)))
        {
            Dictionary<int, IComponent> entityComponentStore = components[typeof(T)];
            if (entityComponentStore.ContainsKey(entityId))
            {
                if (listener != null)
                    listener.onDestruction<T>(entityId);
                entityComponentStore.Remove(entityId);
            }
        }

    }

    internal T? GetComponent<T>(int entityId) where T : IComponent
    {
        if (components.ContainsKey(typeof(T)))
        {
            Dictionary<int, IComponent> entityComponentStore = components[typeof(T)];
            if (entityComponentStore.ContainsKey(entityId))
            {
                return (T)entityComponentStore[entityId];
            }
        }
        return default(T?);
    }

    internal List<int> GetEntitiesWith<T>() where T : IComponent
    {
        if (components.ContainsKey(typeof(T)))
        {
            return components[typeof(T)].Keys.ToList();
        }
        return new List<int>();
    }
}