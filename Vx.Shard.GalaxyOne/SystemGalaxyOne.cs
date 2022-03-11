using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Graphics;
using Vx.Shard.Resources;
using Vx.Shard.Window;

namespace Vx.Shard.GalaxyOne;

public class SystemGalaxyOne : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPosition>();
        componentRegistry.Register<ComponentResourcesShip1Renderer>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder
            .AddCallback<MessageWindowClose>(ExitGame);

        componentStoreListenerBuilder.AddCallback<ComponentResourcesShip1Renderer>(
            (world, entity, component) =>
            {
                entity.GetComponent<ComponentResources>()?.Resources.Add(component.Texture);
                entity.GetComponent<ComponentRenderer>()?.DrawableContainer.AddChild(component.Sprite);
            },
            (world, entity, component) =>
            {
                entity.GetComponent<ComponentResources>()?.Resources.Remove(component.Texture);
                entity.GetComponent<ComponentRenderer>()?.DrawableContainer.RemoveChild(component.Sprite);   
            }
        );
    }

    public void Initialize(World world)
    {
        world.CreateEntity()
            .AddComponent(new ComponentResources())
            .AddComponent(new ComponentRenderer())
            .AddComponent(new ComponentPosition())
            .AddComponent(new ComponentVelocity())
            .AddComponent(new ComponentResourcesShip1Renderer());
    }

    /// <summary>
    /// Close the game.
    /// </summary>
    private void ExitGame(World world, MessageWindowClose _)
    {
        world.GetSingletonComponent<ComponentMainLoop>()!.Running = false;
    }
}