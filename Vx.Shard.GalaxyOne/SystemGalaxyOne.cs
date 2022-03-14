using Vx.Shard.Collision;
using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Physics;
using Vx.Shard.Resources;
using Vx.Shard.Window;

namespace Vx.Shard.GalaxyOne;

public class SystemGalaxyOne : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPosition>();
        componentRegistry.Register<ComponentPhysics>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder
            .AddCallback<MessageWindowClose>(ExitGame);
    }

    public void Initialize(World world)
    {
        var entity = world.CreateEntity()
            .AddComponent(new ComponentResources())
            .AddComponent(new ComponentRenderer())
            .AddComponent(new ComponentPosition())
            .AddComponent(new ComponentPhysics())
            .AddComponent(new ComponentCollision(32))
            .AddComponent(new ComponentGuns())
            .AddComponent(new ComponentShipRenderer("assets/textures/ship1.png", "assets/textures/boost1.png", 5))
            .AddComponent(new ComponentPlayerController());

        var entity2 = world.CreateEntity()
            .AddComponent(new ComponentResources())
            .AddComponent(new ComponentRenderer())
            .AddComponent(new ComponentPosition())
            .AddComponent(new ComponentPhysics())
            .AddComponent(new ComponentGuns())
            .AddComponent(new ComponentCollision(32))
            .AddComponent(new ComponentShipRenderer("assets/textures/ship1.png", "assets/textures/boost1.png", 5))
            .AddComponent(new ComponentEnemyShipController());

        entity2.GetComponent<ComponentPosition>()!.Position = new Vec2(400.0f, 200.0f);

        for (var i = 0; i < 10; i++)
        {
            var entity3 = world.CreateEntity()
                .AddComponent(new ComponentResources())
                .AddComponent(new ComponentRenderer())
                .AddComponent(new ComponentPosition())
                .AddComponent(new ComponentPhysics())
                .AddComponent(new ComponentCollision(32))
                .AddComponent(new ComponentShipRenderer("assets/textures/ship1.png", "assets/textures/boost1.png", 5))
                .AddComponent(new ComponentEnemyShipController());
        }
    }

    /// <summary>
    /// Close the game.
    /// </summary>
    private void ExitGame(World world, MessageWindowClose _)
    {
        world.GetSingletonComponent<ComponentMainLoop>()!.Running = false;
    }
}