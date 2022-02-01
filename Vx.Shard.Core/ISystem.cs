// Copyright (c) Petter Blomkvist (aka. Voxed). All rights reserved.
// License TBD.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vx.Shard.Core.Specs")]
namespace Vx.Shard.Core;

/// <summary>
/// The base interface of all engine systems.
/// </summary>
public interface ISystem
{
    /// <summary>
    /// Configure the message and component listeners.
    /// </summary>
    /// <param name="messageBusListenerBuilder">The message bus listener builder.</param>
    /// <param name="componentStoreListenerBuilder">The component store listener builder.</param>
    public void Configure(MessageBusListenerBuilder messageBusListenerBuilder,
        ComponentStoreListenerBuilder componentStoreListenerBuilder);

    /// <summary>
    /// Do world initializations.
    /// </summary>
    /// <param name="world">The world where the system resides.</param>
    public void Initialize(World world);
}