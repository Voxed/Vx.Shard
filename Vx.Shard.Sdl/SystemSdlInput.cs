using System.ComponentModel;
using SDL2;
using Vx.Shard.Common;
using Vx.Shard.Core;
using Vx.Shard.Window;

namespace Vx.Shard.Sdl;

public class SystemSdlInput : ISystem
{
    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    {
        world.CreateEntity().AddComponent(new ComponentMouse());
    }

    private void Update(World world, MessageUpdate messageUpdate)
    {
        while (SDL.SDL_PollEvent(out var ev) == 1)
        {
            if (ev.type == SDL.SDL_EventType.SDL_WINDOWEVENT)
            {
                if (ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
                {
                    world.Send(new MessageInputWindowClose());
                }
            }
        }

        var componentMouse = world.GetSingletonComponent<ComponentMouse>()!;
        SDL.SDL_GetMouseState(out var x, out var y);
        componentMouse.Position = new Vec2(x, y);
    }
}