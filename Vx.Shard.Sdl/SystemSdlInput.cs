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
            switch (ev.type)
            {
                case SDL.SDL_EventType.SDL_WINDOWEVENT:
                    switch (ev.window.windowEvent)
                    {
                        case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                            world.Send(new MessageWindowClose());
                            break;
                    }
                    break;
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    var pos = new Vec2(ev.motion.x, ev.motion.y);
                    world.Send(new MessageMouseMove(pos));
                    world.GetSingletonComponent<ComponentMouse>()!.Position = pos;
                    break;
            }
        }
    }
}