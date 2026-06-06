using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering;

public class SoftDelete_With_Custom_Column_Name_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModuleWithoutDbFunction>
{
    private readonly IRepository<EntityWithCustomSoftDeleteColumn, Guid> _repository;
    private readonly IDataFilter<ISoftDelete> _softDeleteFilter;

    public SoftDelete_With_Custom_Column_Name_Tests()
    {
        _repository = GetRequiredService<IRepository<EntityWithCustomSoftDeleteColumn, Guid>>();
        _softDeleteFilter = GetRequiredService<IDataFilter<ISoftDelete>>();
    }

    [Fact]
    public async Task SoftDelete_Filter_Should_Work_When_IsDeleted_Has_Custom_Column_Name()
    {
        var ctx = ServiceProvider.GetRequiredService<TestAppDbContext>();
        var isDeletedProperty = ctx.Model.FindEntityType(typeof(EntityWithCustomSoftDeleteColumn))!
            .FindProperty(nameof(ISoftDelete.IsDeleted))!;
        isDeletedProperty.GetColumnName().ShouldBe("custom_is_deleted_column");
        isDeletedProperty.Name.ShouldBe(nameof(ISoftDelete.IsDeleted));

        await _repository.InsertAsync(new EntityWithCustomSoftDeleteColumn { Name = "kept" });
        await _repository.InsertAsync(new EntityWithCustomSoftDeleteColumn { Name = "removed", IsDeleted = true });

        var visible = await _repository.GetListAsync();
        visible.Count.ShouldBe(1);
        visible[0].Name.ShouldBe("kept");

        using (_softDeleteFilter.Disable())
        {
            var all = await _repository.GetListAsync();
            all.Count.ShouldBe(2);
            all.ShouldContain(x => x.Name == "removed" && x.IsDeleted);
        }
    }
}
