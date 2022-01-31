// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

namespace Vx.Shard.Common;

using Core;

/// <summary>
/// The MessageUpdate message. Transmitted by the SystemMainLoop from the main loop.
/// </summary>
public record MessageUpdate : IMessage
{
    /// <summary>
    /// Time passed since the previous MessageUpdate was initially transmitted.
    /// </summary>
    public TimeSpan Delta { get; init; }
}