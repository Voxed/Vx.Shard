// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]
namespace Vx.Shard.Core;

using System.Collections;

/// <summary>
/// A set of entities in the game world.
/// </summary>
public readonly struct EntitySet : IEnumerable<Entity>
{
    private readonly IEnumerable<int> _entities;
    private readonly ComponentStore _store;
    private readonly ComponentRegistry _componentRegistry;

    /// <summary>
    /// Construct a new entity set from a list of entity ids and the component store they exist in.
    /// </summary>
    /// <param name="entities">The entity ids.</param>
    /// <param name="store">The component store where the entities reside.</param>
    /// <param name="componentRegistry">The component registry to use.</param>
    internal EntitySet(IEnumerable<int>? entities, ComponentStore store,
        ComponentRegistry componentRegistry)
    {
        _entities = entities ?? new List<int>();
        _store = store;
        _componentRegistry = componentRegistry;
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        foreach (var entity in _entities)
            yield return new Entity(entity, _store, _componentRegistry);
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
        var newCollection = _store.GetEntitiesWith(componentId);
        if (newCollection == null) return this;
        return new EntitySet(_entities.Intersect(newCollection), _store,
            _componentRegistry);
    }
}