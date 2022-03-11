namespace Vx.Shard.Sdl;

using Core;
using Common;
using SDL2;

/**
 * This system provides necessary SDL initializations.
 */
public class SystemSdl : ISystem
{
    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        messageBusListenerBuilder.AddCallback<MessageUpdate>(Update);
    }

    public void Register(MessageRegistry messageRegistry, ComponentRegistry componentRegistry)
    {
        componentRegistry.Register<ComponentSdl>();
    }

    public void Initialize(World world)
    {
        SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
        SDL_ttf.TTF_Init();
    }

    private void Update(World world, MessageUpdate message)
    {

    }
}