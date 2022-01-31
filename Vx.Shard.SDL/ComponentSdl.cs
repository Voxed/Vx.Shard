namespace Vx.Shard.SDL;

using Core;

public record ComponentSdl : IComponent
{
    public IntPtr Renderer { get; init; }
}