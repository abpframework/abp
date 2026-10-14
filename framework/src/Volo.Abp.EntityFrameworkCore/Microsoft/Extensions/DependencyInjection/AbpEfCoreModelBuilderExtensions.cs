using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.MultiTenancy;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpEfCoreModelBuilderExtensions
{
    public static ModelBuilder ConfigureSoftDeleteDbFunction(this ModelBuilder modelBuilder, MethodInfo methodInfo, AbpEfCoreCurrentDbContext abpEfCoreCurrentDbContext)
    {
        modelBuilder.HasDbFunction(methodInfo)
            .HasTranslation(args =>
            {
                // (bool isDeleted, bool boolParam)
                var isDeleted = args[0];
                var boolParam = args[1];

                if (GetCurrentDbContext(abpEfCoreCurrentDbContext).DataFilter.IsEnabled<ISoftDelete>())
                {
                    // IsDeleted == false. Use the column's TypeMapping so any ValueConverter is applied to the literal.
                    return new SqlBinaryExpression(
                        ExpressionType.Equal,
                        isDeleted,
                        new SqlConstantExpression(false, typeof(bool), isDeleted.TypeMapping ?? boolParam.TypeMapping),
                        boolParam.Type,
                        boolParam.TypeMapping);
                }

                // empty where sql
                return new SqlConstantExpression(true, typeof(bool), boolParam.TypeMapping);
            });

        return modelBuilder;
    }

    public static ModelBuilder ConfigureMultiTenantDbFunction(this ModelBuilder modelBuilder, MethodInfo methodInfo, AbpEfCoreCurrentDbContext abpEfCoreCurrentDbContext)
    {
        modelBuilder.HasDbFunction(methodInfo)
            .HasTranslation(args =>
            {
                // (Guid? tenantId, int? currentTenantId)
                var tenantId = args[0];
                var currentTenantId = args[1];
                var boolParam = args[2];

                if (GetCurrentDbContext(abpEfCoreCurrentDbContext).DataFilter.IsEnabled<IMultiTenant>())
                {
                    // TenantId == CurrentTenantId
                    return new SqlBinaryExpression(
                        ExpressionType.Equal,
                        tenantId,
                        currentTenantId,
                        boolParam.Type,
                        boolParam.TypeMapping);
                }

                // empty where sql
                return new SqlConstantExpression(true, typeof(bool), boolParam.TypeMapping);
            });

        return modelBuilder;
    }

    // The filter state is read while EF Core translates the query, and only AbpEntityQueryProvider sets the current DbContext for that.
    // Queries compiled without it, such as EF.CompileQuery, must fail instead of running without the data filters.
    private static IAbpEfCoreDbFunctionContext GetCurrentDbContext(AbpEfCoreCurrentDbContext abpEfCoreCurrentDbContext)
    {
        return abpEfCoreCurrentDbContext.Context ?? throw new AbpException(
            "The data filters cannot be applied to a query that is compiled outside of AbpEntityQueryProvider, such as EF.CompileQuery or EF.CompileAsyncQuery. " +
            "Run it as a normal LINQ query, or set 'UseDbFunction' of 'AbpEfCoreGlobalFilterOptions' to false.");
    }
}
