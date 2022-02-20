using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Window;

namespace Vx.Shard.SpaceDemo;

public class SystemPlayer : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentPlayer>();
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    {
        world.CreateEntity()
            .AddComponent(new ComponentPlayer())
            .AddComponent(new ComponentPosition(new Vec2(500, 550)));
    }

    private void Update(World world, MessageUpdate message)
    {
        var mainLoopComponent = world.GetSingletonComponent<ComponentMainLoop>()!;
        foreach (var entity in world.GetEntitiesWith<ComponentPlayer>().With<ComponentPosition>())
        {
            var playerComponent = entity.GetComponent<ComponentPlayer>()!;
            var componentPosition = entity.GetComponent<ComponentPosition>()!;
            var mousePosition = world.GetSingletonComponent<ComponentMouse>()!.Position;
            playerComponent.Sprite.Position = componentPosition.Position;
            playerComponent.Speed = (mousePosition - componentPosition.Position) / 5.0f;
            componentPosition.Position += playerComponent.Speed;
            playerComponent.Sprite.Position += playerComponent.Speed;
            playerComponent.Sprite.Rotation = playerComponent.Speed.X / 128.0f;
            if (playerComponent.Spinning)
            {
                playerComponent.SpinningTimer += (float) message.Delta.TotalSeconds * 20.0f;
                playerComponent.Sprite.Resource =
                    playerComponent.SpinningTextures[
                        ((int) playerComponent.SpinningTimer) % playerComponent.SpinningTextures.Count];
            }
            else
            {
                playerComponent.SpinningTimer = 0;
                if (playerComponent.Speed.X > 5.0f)
                {
                    if (playerComponent.Speed.Y < 5.0f)
                    {
                        playerComponent.Sprite.Resource = playerComponent.RightTexture;
                    }
                    else
                    {
                        playerComponent.Sprite.Resource = playerComponent.BackRightTexture;
                    }
                }
                else if (playerComponent.Speed.X < -5.0f)
                {
                    if (playerComponent.Speed.Y < 5.0f)
                    {
                        playerComponent.Sprite.Resource = playerComponent.LeftTexture;
                    }
                    else
                    {
                        playerComponent.Sprite.Resource = playerComponent.BackLeftTexture;
                    }
                }
                else
                {
                    if (playerComponent.Speed.Y < 5.0f)
                    {
                        playerComponent.Sprite.Resource = playerComponent.ForwardTexture;
                    }
                    else
                    {
                        playerComponent.Sprite.Resource = playerComponent.BackForwardTexture;
                    }
                }
            }
        }
    }
}