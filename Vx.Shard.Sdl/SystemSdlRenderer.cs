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
    private record DrawableVisitorSdlContext
    {
        public IntPtr Renderer;

        public Matrix3x2 Transform = Matrix3x2.Identity;

        public SystemSdlRenderer? Sys { get; init; }
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
                Transform = Transform(container, context.Transform)
            };
            container.Children.ToList().ForEach(drawable => drawable.Accept(newContext, this));
        }

        public void VisitSprite(DrawableVisitorSdlContext context, DrawableSprite sprite)
        {
            var texture = ((ResourceTextureSdl) sprite.Resource);

            var newContext = new DrawableVisitorSdlContext
            {
                Renderer = context.Renderer,
                Sys = context.Sys,
                Transform = Transform(sprite, context.Transform)
            };
            
            
            
            
            
            // Huge TODO
            SDL.SDL_Rect sRect;
            SDL.SDL_Rect tRect;

            //var spr = context.Sys!.LoadTexture(sprite.Resource!, out var w, out var h, context.Renderer);
            
            sRect.x = 0;
            sRect.y = 0;
            sRect.w = texture.Width;
            sRect.h = texture.Height;

            var scaled = new Vec2(sRect.w, sRect.h) * sprite.Scaling;
            var pivot = scaled * sprite.Pivot;

            var tr = newContext.Transform;

            
            
            var rot = Math.Atan2(tr.M12, tr.M11);

            var shear = Math.Atan2(tr.M22, tr.M21) - Math.PI / 2 - rot;
            
            tRect.w = (int) (Math.Sqrt(tr.M11 * tr.M11 + tr.M12 * tr.M12) * sRect.w);
            tRect.h = (int) (Math.Sqrt(tr.M21 * tr.M21 + tr.M22 * tr.M22) * sRect.h * Math.Cos(shear));
            tRect.x = (int) (newContext.Transform.M31) - tRect.w/2;
            tRect.y = (int) (newContext.Transform.M32) - tRect.h/2;
            
            IntPtr spr = ((ResourceTextureSdl) sprite.Resource).Texture;

            var p = new SDL.SDL_Point {x = sRect.w/2, y = sRect.h/2};
            
            SDL.SDL_RenderCopyEx(context.Renderer, spr, ref sRect, ref tRect, rot/3.14*180, IntPtr.Zero, 
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
        if (messageLoadResource.Initializer.Type != typeof(ResourceTexture))
            return;

        foreach (var entity in world.GetEntitiesWith<ComponentSdl>())
        {
            var img = SDL_image.IMG_Load(messageLoadResource.Initializer.Path);

            Console.WriteLine($"Loading texture: {messageLoadResource.Initializer.Path}");

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
        if (messageUnloadResource.Type != typeof(ResourceTexture))
            return;

        SDL.SDL_DestroyTexture(((ResourceTextureSdl) messageUnloadResource.Resource).Texture);

        Console.WriteLine($"Unloading texture: {messageUnloadResource.Path}");
    }
}