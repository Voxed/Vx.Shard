namespace Vx.Shard.SDL;

using Core;
using Common;
using SDL2;

/**
 * This system provides necessary SDL initializations.
 */
public class SystemSdl : ISystem
{
    private IntPtr _window, _renderer;

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Initialize(World world)
    {
        SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
        SDL_ttf.TTF_Init();
        _window = SDL.SDL_CreateWindow("Vx.Shard Game Engine",
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            640,
            480,
            0);

        Console.WriteLine(SDL.SDL_GetError());

        _renderer = SDL.SDL_CreateRenderer(_window,
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        Console.WriteLine(SDL.SDL_GetError());

        SDL.SDL_SetRenderDrawBlendMode(_renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

        SDL.SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 255);

        world.CreateEntity().AddComponent(new ComponentSdl
        {
            Renderer = _renderer
        });
    }

    private void Update(World world, MessageUpdate message)
    {
        while (SDL.SDL_PollEvent(out var ev) == 1)
        {
            if (ev.type != SDL.SDL_EventType.SDL_WINDOWEVENT) continue;
            if (ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE)
            {
                world.GetEntitiesWith<ComponentMainLoop>().ToList().ForEach(e =>
                {
                    e.GetComponent<ComponentMainLoop>()!.Running = false;
                });
            }
        }
    }
}