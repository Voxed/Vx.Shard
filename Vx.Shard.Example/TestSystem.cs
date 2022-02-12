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
                        Path = "ship.png",
                        Type = typeof(ResourceTexture)
                    }
                });
                
                entity.AddComponent(new ClientTestComponent
                {
                    Drawable = new DrawableSprite
                    {
                        Resource = entity.GetComponent<ResRef>()!.Test.GetResource<ResourceTexture>(),
                        Scaling = new Vec2(0.5f,0.5f),
                        Pivot = new Vec2(131f/2, 256f/2)
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
        for (var i = 0; i < 20; i++)
        {
            var entity = world.CreateEntity();
            entity.AddComponent(new PositionComponent
            {
                X = 12,
                Y = 34,
                Index = i*10
            });
            entity.GetComponent<PositionComponent>()!.X = 43;
        }
    }

    private void Update(World world, MessageUpdate message)
    {
        foreach (var e in world.GetEntitiesWith<ClientTestComponent>().With<PositionComponent>())
        {
            var pc = e.GetComponent<PositionComponent>()!;
            foreach (var e2 in world.GetEntitiesWith<ComponentMainLoop>())
            {
                var speed = 4.0f;
                var ts = (DateTime.Now - e2.GetComponent<ComponentMainLoop>()!.StartTime).TotalSeconds;
                e.GetComponent<ClientTestComponent>()!.Drawable.Position.X =
                    (int) (Math.Cos(ts +
                                    pc.Index * 0.1) * (350.0 - pc.Index * 1.5)) + 290;
                e.GetComponent<ClientTestComponent>()!.Drawable.Position.Y =
                    (int) (Math.Sin(ts +
                                    pc.Index * 0.1) * (350.0 - pc.Index * 1.5)) + 180;
                e.GetComponent<ClientTestComponent>()!.Drawable.Scaling.X = 0.5f * (float) ((1 + Math.Cos(ts*speed))/2);
                e.GetComponent<ClientTestComponent>()!.Drawable.Scaling.Y = 0.5f + (float) (1 - (1 + Math.Cos(ts*speed))/2);
                e.GetComponent<ClientTestComponent>()!.Drawable.Rotation = 3.14f*(float) Math.Cos(ts*speed*0.5);
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