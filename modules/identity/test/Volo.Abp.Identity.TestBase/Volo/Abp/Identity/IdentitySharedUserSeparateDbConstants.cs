using System;

namespace Volo.Abp.Identity;

// Single source of truth for the predefined tenant ids used by the shared-user separate-database
// test suite. Concrete EF/Mongo test modules and the abstract test class both reference these
// constants directly so the modules don't have to type-couple back into the test class.
public static class IdentitySharedUserSeparateDbConstants
{
    public static readonly Guid TenantAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid TenantBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
}
