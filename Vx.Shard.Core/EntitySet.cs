using System.Collections;

namespace Vx.Shard.Core;

public class EntitySet : IEnumerable<Entity>
{
    List<int> entities;
    ComponentStore store;

    internal EntitySet(List<int> entities, ComponentStore store)
    {
        this.entities = entities;
        this.store = store;
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        for (int i = 0; i < entities.Count; ++i)
            yield return new Entity(entities[i], store);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public EntitySet With<T>() where T : IComponent {
        return new EntitySet(store.GetEntitiesWith<T>().Intersect(this.entities).ToList(), store);
    }
}