namespace Vx.Shard.SDL;

using Vx.Shard.Core;

public record ComponentSDL : IComponent {
    public IntPtr Window {get; init;}
    public IntPtr Renderer {get; init;}
}