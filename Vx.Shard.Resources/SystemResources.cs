using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public class SystemResources : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        componentStoreListenerBuilder.AddSubtypeCallback<IResourceComponent>(
            (world, entity, component) =>
            {
                foreach (var res in component.GetResources())
                {
                    var resInit = new ResourceInitializer
                    {
                        Path = res.Path,
                        Type = res.Type
                    };
                    world.Send(new MessageLoadResource
                    {
                        Initializer = resInit
                    });
                    if (resInit.Resource == null)
                        throw new NullReferenceException($"Failed to initialize resource: {resInit.Path}");
                    res.Resource = resInit.Resource;
                }
            },
            (world, entity, component) =>
            {
                foreach (var res in component.GetResources())
                {
                    world.Send(new MessageUnloadResource
                    {
                        Path = res.Path
                    });
                }
            });
    }

    public void Initialize(World world)
    {
    }
}