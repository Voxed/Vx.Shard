namespace Vx.Shard.Core;

public class ComponentRegistry
{
    private readonly Dictionary<Type, int> _components = new();
    private int _nextComponentId;

    /// <summary>
    /// Register a component type to a component id.
    /// </summary>
    /// <typeparam name="T">The component type to register.</typeparam>
    public void Register<T>() where T : IComponent
    {
        if (!_components.ContainsKey(typeof(T)))
            _components.Add(typeof(T), ++_nextComponentId); // No zero id, that will indicate a null component.
        
        Console.WriteLine("    * Registered component {0} to id {1}", typeof(T).Name, _nextComponentId);
    }

    /// <summary>
    /// Get a component id from a component type.
    /// </summary>
    /// <typeparam name="T">The component type.</typeparam>
    /// <returns>The component id.</returns>
    internal int GetComponentId<T>() where T : IComponent
    {
        return _components.ContainsKey(typeof(T)) ? _components[typeof(T)] : 0;
    }

    /// <summary>
    /// Get the id of all components assignable to T.
    /// </summary>
    /// <typeparam name="T">The type to be assignable to.</typeparam>
    /// <returns>The component ids.</returns>
    internal int[] GetSubtypeComponentIds<T>()
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