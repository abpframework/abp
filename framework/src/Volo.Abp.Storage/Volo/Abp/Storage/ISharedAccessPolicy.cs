using System;

namespace Volo.Abp.Storage
{
    public interface ISharedAccessPolicy
    {
        DateTimeOffset? StartTime { get; }

        DateTimeOffset? ExpiryTime { get; }

        SharedAccessPermissions Permissions { get; }
    }
}