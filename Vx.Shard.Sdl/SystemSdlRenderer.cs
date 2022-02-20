using System.Numerics;
using Vx.Shard.Resources;

namespace Vx.Shard.Sdl;

using Core;
using SDL2;
using Common;
using Graphics;

// TODO: Remake this entire class
public class SystemSdlRenderer : ISystem
{
    private IntPtr _window, _renderer;

    private record DrawableVisitorSdlContext
    {
        public IntPtr Renderer;

        public Matrix3x2 Transform = Matrix3x2.Identity;

        public SystemSdlRenderer? Sys { get; init; }

        public Color Tint = Color.White;

        public float Opacity = 0.0f;
    }

    private class DrawableVisitorSdl : IDrawableVisitor<DrawableVisitorSdlContext>
    {
        private Matrix3x2 Transform(IDrawable drawable, Matrix3x2 old)
        {
            return
                Matrix3x2.CreateScale(drawable.Scaling.X, drawable.Scaling.Y) *
                Matrix3x2.CreateRotation(drawable.Rotation, new Vector2(
                    0,
                    0)) *
                Matrix3x2.CreateTranslation(drawable.Position.X,
                    drawable.Position.Y) *
                old;
        }

        public void VisitContainer(DrawableVisitorSdlContext context, DrawableContainer container)
        {
            var newContext = new DrawableVisitorSdlContext
            {
                Renderer = context.Renderer,
                Sys = context.Sys,
                Transform = Transform(container, context.Transform),
                Tint = container.Tint * context.Tint,
                Opacity = container.Opacity + context.Opacity
            };
            container.Children.ToList().ForEach(drawable => drawable.Accept(newContext, this));
        }

        public void VisitSprite(DrawableVisitorSdlContext context, DrawableSprite sprite)
        {
            var texture = ((ResourceTextureSdl) sprite.Resource.Resource);

            var newContext = new DrawableVisitorSdlContext
            {
                Renderer = context.Renderer,
                Sys = context.Sys,
                Transform = Transform(sprite, context.Transform),
                Tint = sprite.Tint * context.Tint,
                Opacity = sprite.Opacity + context.Opacity
            };


            // Huge TODO
            SDL.SDL_Rect sRect;
            SDL.SDL_Rect tRect;

            //var spr = context.Sys!.LoadTexture(sprite.Resource!, out var w, out var h, context.Renderer);

            sRect.x = 0;
            sRect.y = 0;
            sRect.w = texture.Width;
            sRect.h = texture.Height;

            var tr = newContext.Transform;


            var rot = Math.Atan2(tr.M12, tr.M11);

            var shear = Math.Atan2(tr.M22, tr.M21) - Math.PI / 2 - rot;


            var scaleX = Math.Sqrt(tr.M11 * tr.M11 + tr.M12 * tr.M12);
            var scaleY = Math.Sqrt(tr.M21 * tr.M21 + tr.M22 * tr.M22);

            tRect.w = (int) (scaleX * sRect.w);
            tRect.h = (int) (scaleY * sRect.h * Math.Cos(shear));
            tRect.x = (int) (newContext.Transform.M31);
            tRect.y = (int) (newContext.Transform.M32);

            IntPtr spr = ((ResourceTextureSdl) sprite.Resource.Resource).Texture;

            var p = new SDL.SDL_Point {x = sRect.w / 2, y = sRect.h / 2};

            switch (sprite.BlendMode)
            {
                case BlendMode.Add:
                    SDL.SDL_SetTextureBlendMode(spr, SDL.SDL_BlendMode.SDL_BLENDMODE_ADD);
                    break;
                case BlendMode.Normal:
                    SDL.SDL_SetTextureBlendMode(spr, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                    break;
                case BlendMode.Modulate:
                    SDL.SDL_SetTextureBlendMode(spr, SDL.SDL_BlendMode.SDL_BLENDMODE_MOD);
                    break;
                case BlendMode.Multiply:
                    SDL.SDL_SetTextureBlendMode(spr, SDL.SDL_BlendMode.SDL_BLENDMODE_MUL);
                    break;
            }

            var center = new SDL.SDL_Point();
            center.x = (int) (tRect.w * sprite.Pivot.X);
            center.y = (int) (tRect.h * sprite.Pivot.Y);

            tRect.x -= center.x;
            tRect.y -= center.y;

            SDL.SDL_SetTextureColorMod(spr, (byte) (newContext.Tint.R * 255), (byte) (newContext.Tint.G * 255),
                (byte) (newContext.Tint.B * 255));
            SDL.SDL_SetTextureAlphaMod(spr, (byte) (255 - 255 * newContext.Opacity));
            SDL.SDL_RenderCopyEx(context.Renderer, spr, ref sRect, ref tRect, rot / 3.14 * 180,
                ref center,
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
        messageBusListenerBuilder.AddCallback<MessageLoadResource>(LoadTexture);
        messageBusListenerBuilder.AddCallback<MessageUnloadResource>(UnloadTexture);
    }

    public void Initialize(World world)
    {
        _window = SDL.SDL_CreateWindow("Vx.Shard Game Engine",
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            1000, 700,
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

        world.GetSingletonComponent<ComponentGraphicsScene>()!.Size = new Vec2(1000, 700);
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

    private void LoadTexture(World world, MessageLoadResource messageLoadResource)
    {
        if (messageLoadResource.Initializer.Type != "texture")
            return;

        foreach (var entity in world.GetEntitiesWith<ComponentSdl>())
        {
            Console.WriteLine($"Loading texture: {messageLoadResource.Initializer.Path}");

            var img = SDL_image.IMG_Load(messageLoadResource.Initializer.Path);

            if (img == IntPtr.Zero)
            {
                Console.WriteLine($"Error when loading image: {SDL.SDL_GetError()}");
                return;
            }

            var texture = SDL.SDL_CreateTextureFromSurface(entity.GetComponent<ComponentSdl>()!.Renderer, img);

            SDL.SDL_SetTextureBlendMode(texture, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            SDL.SDL_QueryTexture(texture, out _, out _, out var width, out var height);

            var resourceTexture = new ResourceTextureSdl
            {
                Texture = texture,
                Width = width,
                Height = height
            };

            messageLoadResource.Initializer.Resource = resourceTexture;
        }
    }

    private void UnloadTexture(World world, MessageUnloadResource messageUnloadResource)
    {
        if (messageUnloadResource.Reference.Type != "texture")
            return;

        SDL.SDL_DestroyTexture(((ResourceTextureSdl) messageUnloadResource.Reference.Resource).Texture);

        Console.WriteLine($"Unloading texture: {messageUnloadResource.Reference.Path}");
    }
}