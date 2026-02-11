using System;

namespace Volo.Abp.Domain.Repositories;

public interface IEntityChangeTrackingProvider
{
    bool? Enabled { get; }

    IDisposable Change(bool? enabled);
}
