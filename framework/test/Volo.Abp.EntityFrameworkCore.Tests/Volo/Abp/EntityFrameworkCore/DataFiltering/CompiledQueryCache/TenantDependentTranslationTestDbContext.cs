using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class TenantDependentTranslationTestDbContext : AbpDbContext<TenantDependentTranslationTestDbContext>
{
    public DbSet<TenantDependentTranslationTestRow> Rows { get; set; }

    public TenantDependentTranslationTestDbContext(DbContextOptions<TenantDependentTranslationTestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TenantDependentTranslationTestRow>(builder =>
        {
            builder.ToTable("CustomTranslationRows");
            builder.Property(row => row.Name).HasMaxLength(100);
            builder.HasQueryFilter(row => MatchesCurrentTenant(row.OwnerTenantId, true));
        });

        var currentDbContext = this.GetService<AbpEfCoreCurrentDbContext>();
        modelBuilder.HasDbFunction(typeof(TenantDependentTranslationTestDbContext)
                .GetMethod(nameof(MatchesCurrentTenant)))
            .HasTranslation(arguments =>
            {
                var ownerTenantId = arguments[0];
                var boolParameter = arguments[1];
                var currentTenantId = currentDbContext.Context.CurrentTenant.Id;

                return new SqlBinaryExpression(
                    ExpressionType.Equal,
                    ownerTenantId,
                    new SqlConstantExpression(currentTenantId, typeof(Guid?), ownerTenantId.TypeMapping),
                    boolParameter.Type,
                    boolParameter.TypeMapping);
            });
    }

    public static bool MatchesCurrentTenant(Guid? ownerTenantId, bool boolParameter)
    {
        throw new NotSupportedException(AbpEfCoreDataFilterDbFunctionMethods.NotSupportedExceptionMessage);
    }

    public override string GetCompiledQueryCacheKey()
    {
        return $"{base.GetCompiledQueryCacheKey()}:{CurrentTenantId?.ToString() ?? "Null"}";
    }
}
