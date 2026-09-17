using System;
using System.Linq;
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

// Regression for the soft-delete DbFunction translator hardcoding a bool false literal
// with the bool TypeMapping instead of routing the literal through the IsDeleted
// property's ValueConverter. EntityWithIntSoftDelete maps false->5, true->9, so the
// buggy SQL "is_deleted = 0" matches no row (returns 0) while the correct SQL
// "is_deleted = 5" matches the not-deleted row (returns 1).
public class SoftDelete_DbFunction_Bool_Literal_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModule>
{
    private readonly IRepository<EntityWithIntSoftDelete, Guid> _repository;
    private readonly IDataFilter<ISoftDelete> _softDeleteFilter;

    public SoftDelete_DbFunction_Bool_Literal_Tests()
    {
        _repository = GetRequiredService<IRepository<EntityWithIntSoftDelete, Guid>>();
        _softDeleteFilter = GetRequiredService<IDataFilter<ISoftDelete>>();
    }

    [Fact]
    public async Task SoftDelete_Filter_Should_Route_False_Literal_Through_ValueConverter()
    {
        await _repository.InsertAsync(new EntityWithIntSoftDelete { Name = "kept" });
        await _repository.InsertAsync(new EntityWithIntSoftDelete { Name = "removed", IsDeleted = true });

        var visible = await _repository.GetListAsync();
        visible.Count.ShouldBe(1);
        visible[0].Name.ShouldBe("kept");

        using (_softDeleteFilter.Disable())
        {
            var all = await _repository.GetListAsync();
            all.Count.ShouldBe(2);
        }
    }
}
