using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering;

public class MultiTenant_With_Custom_Column_Name_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModuleWithoutDbFunction>
{
    private readonly IRepository<EntityWithCustomTenantIdColumn, Guid> _repository;
    private readonly IDataFilter<IMultiTenant> _multiTenantFilter;

    public MultiTenant_With_Custom_Column_Name_Tests()
    {
        _repository = GetRequiredService<IRepository<EntityWithCustomTenantIdColumn, Guid>>();
        _multiTenantFilter = GetRequiredService<IDataFilter<IMultiTenant>>();
    }

    [Fact]
    public async Task MultiTenant_Filter_Should_Work_When_TenantId_Has_Custom_Column_Name()
    {
        var ctx = ServiceProvider.GetRequiredService<TestAppDbContext>();
        var tenantIdProperty = ctx.Model.FindEntityType(typeof(EntityWithCustomTenantIdColumn))!
            .FindProperty(nameof(IMultiTenant.TenantId))!;
        tenantIdProperty.GetColumnName().ShouldBe(EntityWithCustomTenantIdColumn.TenantIdColumnName);
        tenantIdProperty.Name.ShouldBe(nameof(IMultiTenant.TenantId));

        using (_multiTenantFilter.Disable())
        {
            await _repository.InsertAsync(new EntityWithCustomTenantIdColumn { Name = "host", TenantId = null });
            await _repository.InsertAsync(new EntityWithCustomTenantIdColumn { Name = "tenant", TenantId = Guid.NewGuid() });

            var all = await _repository.GetListAsync();
            all.Count.ShouldBe(2);
        }

        var hostScoped = await _repository.GetListAsync();
        hostScoped.Count.ShouldBe(1);
        hostScoped[0].Name.ShouldBe("host");
    }

    [Fact]
    public void MultiTenant_Filter_Should_Reference_TenantId_By_Clr_Property_Name()
    {
        var ctx = ServiceProvider.GetRequiredService<TestAppDbContext>();
        var entityType = ctx.Model.FindEntityType(typeof(EntityWithCustomTenantIdColumn))!;

#pragma warning disable EF1001
        var annotation = entityType.FindAnnotation(CoreAnnotationNames.QueryFilter);
#pragma warning restore EF1001
        annotation.ShouldNotBeNull();

        var args = FilterExpressionPropertyNameInspector.GetEfPropertyStringArgs(annotation.Value!);
        args.ShouldNotBeEmpty();
        foreach (var arg in args)
        {
            arg.ShouldBe(nameof(IMultiTenant.TenantId));
        }
    }
}
