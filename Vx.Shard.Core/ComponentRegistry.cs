namespace Vx.Shard.Core;

public class ComponentRegistry
{
    private readonly Dictionary<Type, int> _components = new();
    private int _nextComponentId;

    internal ComponentRegistry()
    {
    }

    public void Register<T>() where T : IComponent
    {
        if (!_components.ContainsKey(typeof(T)))
            _components.Add(typeof(T), ++_nextComponentId); // No zero id, that will indicate a null component.
        
        Console.WriteLine("    * Registered component {0} to id {1}", typeof(T).Name, _nextComponentId);
    }

    internal int GetComponentId<T>() where T : IComponent
    {
        return _components.ContainsKey(typeof(T)) ? _components[typeof(T)] : 0;
    }

    internal int[] GetSubclassComponentIds<T>()
    {
        List<int> ids = new();
        foreach (var pair in _components)
        {
            if (typeof(T).IsAssignableFrom(pair.Key))
                ids.Add(pair.Value);
        }
        return ids.ToArray();
    }
}