using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;

namespace Vx.Shard.SpaceDemo;

public class SystemWorld : ISystem
{
    private Random _random = new();

    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentStar>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    {
        var sceneSize = world.GetSingletonComponent<ComponentGraphicsScene>()?.Size ?? Vec2.One;
        for (var i = 0; i < 256; i++)
        {
            world.CreateEntity()
                .AddComponent(new ComponentStar((float) _random.NextDouble(), (float) _random.NextDouble(),
                    (float) _random.NextDouble(), (float) _random.NextDouble()))
                .AddComponent(new ComponentPosition(new Vec2(_random.Next((int) sceneSize.X),
                    _random.Next((int) sceneSize.Y * 2) - sceneSize.Y)));
        }
    }

    private void Update(World world, MessageUpdate message)
    {
        var sceneSize = world.GetSingletonComponent<ComponentGraphicsScene>()?.Size ?? Vec2.One;
        foreach (var star in world.GetEntitiesWith<ComponentStar>().With<ComponentPosition>())
        {
            var positionComponent = star.GetComponent<ComponentPosition>()!;
            var starComponent = star.GetComponent<ComponentStar>()!;
            positionComponent.Position.Y +=
                (float) message.Delta.TotalSeconds * 2048.0f * (starComponent.Speed / 4.0f + 0.2f);
            starComponent.Sprite.Position = positionComponent.Position;
            if (positionComponent.Position.Y > sceneSize.Y + 32f)
            {
                positionComponent.Position.Y = -_random.Next((int) sceneSize.Y);
                positionComponent.Position.X = _random.Next((int) sceneSize.X);
                starComponent.Redness = (float) _random.NextDouble();
                starComponent.Size = (float) _random.NextDouble();
                starComponent.Speed = (float) _random.NextDouble();
                starComponent.Opacity = (float) _random.NextDouble();
            }
        }
    }
}