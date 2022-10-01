using System;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

public interface IHasRemoteModificationTime
{
    /// <summary>
    /// The last modified time for the synchronized remote entity.
    /// </summary>
    DateTime? RemoteLastModificationTime { get; }
}