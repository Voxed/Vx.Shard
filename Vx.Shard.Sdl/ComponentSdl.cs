namespace Vx.Shard.Sdl;

using Core;

public record ComponentSdl : IComponent
{
    public IntPtr Renderer { get; init; }
}