using System.Collections.Generic;

namespace Volo.Abp.Authorization.Permissions.Resources;

public interface IHasResourcePermissions : IKeyedObject
{
    Dictionary<string, bool> ResourcePermissions { get; }
}
