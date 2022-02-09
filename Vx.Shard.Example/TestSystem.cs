using Vx.Shard.Graphics;
using Vx.Shard.Resources;
using Vx.Shard.Window;

namespace Vx.Shard.Example;

using Core;
using Common;

public class TestSystem : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ResRef>();
        componentRegistry.Register<ClientTestComponent>();
        componentRegistry.Register<PositionComponent>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
        messageBusListenerBuilder.AddCallback<MessageInputWindowClose>(Close);
        componentStoreListenerBuilder.AddCallback<PositionComponent>(
            (_, entity, component) =>
            {
                entity.AddComponent(new ResRef
                {   
                    Test = new ResourceReference
                    {
                        Path = component.Index % 2 == 0 ? "test.png" : "test2.png",
                        Type = typeof(ResourceTexture)
                    }
                });
                
                entity.AddComponent(new ClientTestComponent
                {
                    Drawable = new DrawableSprite
                    {
                        Resource = entity.GetComponent<ResRef>()!.Test.GetResource<ResourceTexture>()
                    }
                });
            },
            (_, entity, _) =>
            {
                entity.RemoveComponent<ClientTestComponent>();
                entity.RemoveComponent<ResRef>();
            }
        );
    }

    public void Initialize(World world)
    {
        for (var i = 0; i < 200; i++)
        {
            var entity = world.CreateEntity();
            if (i < 5) continue;
            entity.AddComponent(new PositionComponent
            {
                X = 12,
                Y = 34,
                Index = i
            });
            entity.GetComponent<PositionComponent>()!.X = 43;
        }
    }

    private void Update(World world, MessageUpdate message)
    {
        var i = 0;
        foreach (var e in world.GetEntitiesWith<ClientTestComponent>().With<PositionComponent>())
        {
            i++;
            foreach (var e2 in world.GetEntitiesWith<ComponentMainLoop>())
            {
                e.GetComponent<ClientTestComponent>()!.Drawable.Position.X =
                    (int) (Math.Cos((DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds +
                                    i * 0.1) * (350.0 - i * 1.5)) + 290;
                e.GetComponent<ClientTestComponent>()!.Drawable.Position.Y =
                    (int) (Math.Sin((DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds +
                                    i * 0.1) * (350.0 - i * 1.5)) + 180;
            }
        }
    }

    private void Close(World world, MessageInputWindowClose messageInputWindowClose)
    {
        foreach (var p in world.GetEntitiesWith<PositionComponent>())
        {
            p.RemoveComponent<PositionComponent>();
        }
        
        world.GetSingletonComponent<ComponentMainLoop>()!.Running = false;
    }
}