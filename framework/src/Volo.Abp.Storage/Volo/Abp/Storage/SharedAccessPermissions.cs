using System;

namespace Volo.Abp.Storage
{
    [Flags]
    public enum SharedAccessPermissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,
        List = 8,
        Add = 16,
        Create = 32,
        All = Read | Write | Delete | List | Add | Create
    }
}