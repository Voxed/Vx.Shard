using Vx.Shard.Core;

namespace Vx.Shard.Resources;

public class SystemResources : ISystem
{
    private readonly Dictionary<string, int> _resourceCounter = new();
    private readonly Dictionary<string, IResource> _resources = new();

    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        messageRegistry.Register<MessageLoadResource>();
        messageRegistry.Register<MessageUnloadResource>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        componentStoreListenerBuilder.AddSubtypeCallback<IResourceComponent>(
            (world, entity, component) =>
            {
                foreach (var res in component.GetResources())
                {
                    _resourceCounter.TryGetValue(res.Path, out var resCount);
                    if (resCount > 0)
                    {
                        res.Resource = _resources[res.Path];
                    }
                    else
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
                        _resources[res.Path] = resInit.Resource;
                    }
                    _resourceCounter[res.Path] = resCount + 1;
                }
            },
            (world, entity, component) =>
            {
                foreach (var res in component.GetResources())
                {
                    _resourceCounter[res.Path]--;
                    if (_resourceCounter[res.Path] <= 0)
                    {
                        world.Send(new MessageUnloadResource
                        {
                            Resource = res.Resource!
                        });
                    }
                }
            });
    }

    public void Initialize(World world)
    {
    }
}