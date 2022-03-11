using System.Collections.ObjectModel;
using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public class ComponentResources : IComponent
{
    public ObservableCollection<ResourceReference> Resources { get; } = new ObservableCollection<ResourceReference>();

    public ComponentResources(List<ResourceReference> resources)
    {
        foreach (var res in resources)
        {
            Resources.Add(res);
        }
    }

    public ComponentResources()
    {
        
    }
}