// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]

namespace Vx.Shard.Core;

using System.Collections;

/// <summary>
/// A set of entities in the game world.
/// </summary>
public class EntitySet : IEnumerable<Entity>
{
    private readonly List<int> _entities;
    private readonly ComponentStore _store;
    private readonly ComponentRegistry _componentRegistry;

    /// <summary>
    /// Construct a new entity set from a list of entity ids and the component store they exist in.
    /// </summary>
    /// <param name="entities">The entity ids.</param>
    /// <param name="store">The component store where the entities reside.</param>
    internal EntitySet(List<int> entities, ComponentStore store, ComponentRegistry componentRegistry)
    {
        _entities = entities;
        _store = store;
        _componentRegistry = componentRegistry;
    }

    /// <summary>
    /// Get an enumerator to enumerate over the entities in the set.
    /// </summary>
    /// <returns>The entity enumerator.</returns>
    public IEnumerator<Entity> GetEnumerator()
    {
        return new EntitySetEnumerator(_entities, _store, _componentRegistry);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Filter out entities in the set who have a specified component.
    /// </summary>
    /// <typeparam name="T">The component type to query for.</typeparam>
    /// <returns>A new set with entities who have the specified component.s</returns>
    public EntitySet With<T>() where T : IComponent
    {
        var componentId = _componentRegistry.GetComponentId<T>();
        return new EntitySet(_store.GetEntitiesWith(componentId).Intersect(_entities).ToList(), _store,
            _componentRegistry);
    }
}

/// <summary>
/// Enumerator of an EntitySet.
/// </summary>
public class EntitySetEnumerator : IEnumerator<Entity>
{
    private readonly ComponentRegistry _componentRegistry;
    private readonly ComponentStore _store;
    private readonly List<int> _entities;
    private int _position = -1;

    /// <summary>
    /// Construct a new EntitySetEnumerator from entities and the component store where they reside.
    /// </summary>
    /// <param name="list">The entities.</param>
    /// <param name="store">The component store.</param>
    internal EntitySetEnumerator(List<int> list, ComponentStore store, ComponentRegistry componentRegistry)
    {
        _entities = list;
        _store = store;
        _componentRegistry = componentRegistry;
    }

    // Below follows standard implemented enumerator methods.
    public bool MoveNext()
    {
        _position++;
        return _position < _entities.Count;
    }

    public void Reset()
    {
        _position = -1;
    }

    object IEnumerator.Current => Current;

    public Entity Current => new(_entities[_position], _store, _componentRegistry);

    public void Dispose()
    {
    }
}