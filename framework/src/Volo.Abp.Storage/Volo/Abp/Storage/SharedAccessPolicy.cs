using System;

namespace Volo.Abp.Storage
{
    public class SharedAccessPolicy : ISharedAccessPolicy
    {
        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }

        public SharedAccessPermissions Permissions { get; set; }
    }
}
