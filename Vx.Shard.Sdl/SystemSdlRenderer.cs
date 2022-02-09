namespace Vx.Shard.Sdl;

using Core;
using SDL2;
using Common;
using Graphics;

// TODO: Remake this entire class
public class SystemSdlRenderer : ISystem
{
    private readonly Dictionary<string, IntPtr> _spriteBuffer = new();

    private IntPtr LoadTexture(string path, out int w, out int h, IntPtr rend)
    {
        var ret = LoadTexture(path, rend);

        SDL.SDL_QueryTexture(ret, out _, out _, out w, out h);

        return ret;
    }


    private IntPtr LoadTexture(string path, IntPtr rend)
    {
        if (_spriteBuffer.ContainsKey(path))
        {
            return _spriteBuffer[path];
        }

        var img = SDL_image.IMG_Load(path);

        Console.WriteLine(SDL_image.IMG_GetError());

        _spriteBuffer[path] = SDL.SDL_CreateTextureFromSurface(rend, img);

        SDL.SDL_SetTextureBlendMode(_spriteBuffer[path], SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

        return img;
    }


    private record DrawableVisitorSdlContext
    {
        public IntPtr Renderer;
        public Vec2 Position { get; init; } = Vec2.Zero;

        public SystemSdlRenderer? Sys { get; init; }
    }

    private class DrawableVisitorSdl : IDrawableVisitor<DrawableVisitorSdlContext>
    {
        public void VisitContainer(DrawableVisitorSdlContext context, DrawableContainer container)
        {
            var newContext = new DrawableVisitorSdlContext
            {
                Renderer = context.Renderer,
                Position = context.Position + container.Position,
                Sys = context.Sys
            };

            container.Children.ToList().ForEach(drawable => drawable.Accept(newContext, this));
        }

        public void VisitSprite(DrawableVisitorSdlContext context, DrawableSprite sprite)
        {
            // Huge TODO
            SDL.SDL_Rect sRect;
            SDL.SDL_Rect tRect;

            //var spr = context.Sys!.LoadTexture(sprite.Resource!, out var w, out var h, context.Renderer);

            sRect.x = 0;
            sRect.y = 0;
            sRect.w = 64;
            sRect.h = 64;

            tRect.x = (int) (context.Position.X + sprite.Position.X);
            tRect.y = (int) (context.Position.Y + sprite.Position.Y);
            tRect.w = sRect.w;
            tRect.h = sRect.h;

            IntPtr spr = ((ResourceTextureSdl) sprite.Resource).Texture;
            
            SDL.SDL_RenderCopyEx(context.Renderer, spr, ref sRect, ref tRect, 0, IntPtr.Zero,
                SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }

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
    }

    private void Update(World world, MessageUpdate message)
    {
        foreach (var e in world.GetEntitiesWith<ComponentSdl>())
        {
            var context = new DrawableVisitorSdlContext
            {
                Renderer = e.GetComponent<ComponentSdl>()!.Renderer,
                Sys = this
            };

            SDL.SDL_SetRenderDrawColor(context.Renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(context.Renderer);
            foreach (var entity in world.GetEntitiesWith<ComponentGraphicsScene>())
            {
                var scene = entity.GetComponent<ComponentGraphicsScene>()!;
                var root = scene.Root;
                root.Accept(context, new DrawableVisitorSdl());
            }

            SDL.SDL_RenderPresent(context.Renderer);
        }
    }
}