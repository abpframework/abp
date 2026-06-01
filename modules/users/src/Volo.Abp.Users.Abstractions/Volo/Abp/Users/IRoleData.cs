using System;
using Volo.Abp.Data;

namespace Volo.Abp.Users;

public interface IRoleData : IHasExtraProperties
{
    Guid Id { get; }

    Guid? TenantId { get; }

    string Name { get; }

    bool IsDefault { get;  }

    bool IsStatic { get; }

    bool IsPublic { get; }
}
