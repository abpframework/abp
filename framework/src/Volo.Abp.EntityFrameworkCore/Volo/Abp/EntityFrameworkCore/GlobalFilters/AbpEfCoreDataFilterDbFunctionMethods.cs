using System;
using System.Reflection;

namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public static class AbpEfCoreDataFilterDbFunctionMethods
{
    public static bool SoftDeleteFilter(bool isDeleted, bool boolParam)
    {
        throw new NotSupportedException("This method should be replaced by the database function call.");
    }

    public static MethodInfo SoftDeleteFilterMethodInfo => typeof(AbpEfCoreDataFilterDbFunctionMethods).GetMethod(nameof(SoftDeleteFilter))!;

    public static bool MultiTenantFilter(Guid? tenantId, Guid? currentTenantId, bool boolParam)
    {
        throw new NotSupportedException("This method should be replaced by the database function call.");
    }

    public static MethodInfo MultiTenantFilterMethodInfo => typeof(AbpEfCoreDataFilterDbFunctionMethods).GetMethod(nameof(MultiTenantFilter))!;
}
