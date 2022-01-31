namespace Vx.Shard.Core;

public class Entity
{

    public readonly int Id;
    private readonly ComponentStore store;

    internal Entity(int id, ComponentStore store)
    {
        this.Id = id;
        this.store = store;
    }

    public void AddComponent<T>(T component) where T : IComponent
    {
        store.AddComponent(Id, component);
    }

    public void RemoveComponent<T>() where T : IComponent {
        store.RemoveComponent<T>(Id);
    }

    public T? GetComponent<T>() where T : IComponent
    {
        return store.GetComponent<T>(Id);
    }

}