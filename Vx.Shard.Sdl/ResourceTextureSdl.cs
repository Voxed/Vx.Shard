using Vx.Shard.Graphics;

namespace Vx.Shard.Sdl;

public class ResourceTextureSdl : ResourceTexture
{
    public IntPtr Texture { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
}