using System.Collections;
using System.Collections.ObjectModel;
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
        componentRegistry.Register<ComponentResources>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        componentStoreListenerBuilder.AddCallback<ComponentResources>(
            (world, entity, component) =>
            {
                Load(world, component.Resources);
                component.Resources.CollectionChanged += (sender, args) =>
                {
                    if (args.NewItems != null) Load(world, args.NewItems);
                    if (args.OldItems != null) Unload(world, args.OldItems);
                };
            },
            (world, entity, component) =>
            {
                Unload(world, component.Resources);
            });
    }

    private void Load(World world, IList resources)
    {
        foreach (ResourceReference res in resources)
        {
            _resourceCounter.TryGetValue(res.Path, out var resCount);
            if (resCount > 0)
            {
                res.SetResource(_resources[res.Path]);
            }
            else
            {
                var resInit = new ResourceInitializer(res.Path, res.Type);
                world.Send(new MessageLoadResource(
                    resInit
                ));
                if (resInit.Resource == null)
                    throw new NullReferenceException($"Failed to initialize resource: {resInit.Path}");
                res.SetResource(resInit.Resource);
                _resources[res.Path] = resInit.Resource;
            }

            _resourceCounter[res.Path] = resCount + 1;
        }
    }

    private void Unload(World world, IList resources)
    {
        foreach (ResourceReference res in resources)
        {
            _resourceCounter[res.Path]--;
            if (_resourceCounter[res.Path] <= 0)
            {
                world.Send(new MessageUnloadResource(res));
            }
        }   
    }

    public void Initialize(World world)
    {
    }
}