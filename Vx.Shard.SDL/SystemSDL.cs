namespace Vx.Shard.SDL;

using Vx.Shard.Core;
using Vx.Shard.Common;
using SDL2;

/**
 * This system provides necessary SDL initializations.
 */
public class SystemSDL : ISystem
{
    private IntPtr window, renderer;
    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(update);
    }

    public void Initialize(World world)
    {
        SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
        SDL_ttf.TTF_Init();
        window = SDL.SDL_CreateWindow("Vx.Shard Game Engine",
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            640,
            480,
            0);

        Console.WriteLine("sTART");
        Console.WriteLine(SDL.SDL_GetError());

        renderer = SDL.SDL_CreateRenderer(window,
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        Console.WriteLine(SDL.SDL_GetError());


        SDL.SDL_SetRenderDrawBlendMode(renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

        SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);

        world.CreateEntity().AddComponent(new ComponentSDL
        {
            Window = window,
            Renderer = renderer
        });

        Console.WriteLine(SDL.SDL_GetError());
    }

    public void update(World world, MessageUpdate message)
    {
        SDL.SDL_Event ev;
        while(SDL.SDL_PollEvent(out ev) == 1) {
            if(ev.type == SDL.SDL_EventType.SDL_WINDOWEVENT) {
                if(ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE) {
                    world.GetEntitiesWith<ComponentMainLoop>().ToList().ForEach(e => {
                        e.GetComponent<ComponentMainLoop>()!.Running = false;
                    });
                }
            }   
        }
    }
}