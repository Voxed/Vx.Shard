namespace Vx.Shard.SDL;

using Vx.Shard.Core;
using SDL2;
using Vx.Shard.Common;
using Vx.Shard.Graphics;

public class SDLDrawablesSystem : ISystem
{
    private Dictionary<string, IntPtr> spriteBuffer = new Dictionary<string, IntPtr>();

    public IntPtr loadTexture(string path, out int w, out int h, IntPtr rend)
    {
        IntPtr ret;
        uint format;
        int access;

        ret = loadTexture(path, rend);

        SDL.SDL_QueryTexture(ret, out format, out access, out w, out h);

        return ret;

    }


    public IntPtr loadTexture(string path, IntPtr _rend)
    {
        IntPtr img;

        if (spriteBuffer.ContainsKey(path))
        {
            return spriteBuffer[path];
        }

        img = SDL_image.IMG_Load(path);

        Console.WriteLine(SDL_image.IMG_GetError());

        /*Debug.getInstance().log("IMG_Load: " + SDL_image.IMG_GetError());*/

        spriteBuffer[path] = SDL.SDL_CreateTextureFromSurface(_rend, img);

        SDL.SDL_SetTextureBlendMode(spriteBuffer[path], SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

        return img;

    }








    private record DrawableVisitorSDLContext
    {
        public IntPtr Renderer;
        public Vec2 Position { get; init; } = Vec2.Zero;

        public SDLDrawablesSystem? sys { get; init; }
    }

    private class DrawableVisitorSDL : IDrawableVisitor<DrawableVisitorSDLContext>
    {
        public void VisitContainer(DrawableVisitorSDLContext context, DrawableContainer container)
        {
            DrawableVisitorSDLContext newContext = new DrawableVisitorSDLContext
            {
                Renderer = context.Renderer,
                Position = context.Position + container.Position,
                sys = context.sys
            };

            container.Children.ToList().ForEach(drawable => drawable.Accept(newContext, this));
        }

        public void VisitSprite(DrawableVisitorSDLContext context, DrawableSprite sprite)
        {
            SDL.SDL_Rect sRect;
            SDL.SDL_Rect tRect;

            //var sprite = loadTexture(trans);

            int w, h;

            var spr = context.sys!.loadTexture(sprite.TexturePath!, out w, out h, context.Renderer);

            sRect.x = 0;
            sRect.y = 0;
            sRect.w = (int)(w);
            sRect.h = (int)(h);

            tRect.x = (int)(context.Position.X + sprite.Position.X);
            tRect.y = (int)(context.Position.Y + sprite.Position.Y);
            tRect.w = sRect.w;
            tRect.h = sRect.h;

            SDL.SDL_RenderCopyEx(context.Renderer, spr, ref sRect, ref tRect, 0, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }

    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder, ComponentStoreListenerBuilder componentStoreListenerBuilder)
    {
        Console.WriteLine("Config!!!");
        messageBusListenerBuilder.AddCallback<MessageUpdate>(update);
    }

    public void Initialize(World world)
    {

    }

    public void update(World world, MessageUpdate message)
    {
        world.GetEntitiesWith<ComponentSDL>().ToList().ForEach(e =>
        {
            DrawableVisitorSDLContext context = new DrawableVisitorSDLContext
            {
                Renderer = e.GetComponent<ComponentSDL>()!.Renderer,
                sys = this
            };

            SDL.SDL_SetRenderDrawColor(context.Renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(context.Renderer);
            world.GetEntitiesWith<ComponentGraphicsScene>().ToList().ForEach(entity =>
            {
                ComponentGraphicsScene scene = entity.GetComponent<ComponentGraphicsScene>()!;
                DrawableContainer root = scene.root;
                root.Accept(context, new DrawableVisitorSDL());
            });

            SDL.SDL_RenderPresent(context.Renderer);
        });
    }
}