using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering;

public class EfCore_Custom_Filter_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModule>
{
    private readonly IBasicRepository<Category, Guid> _categoryRepository;

    public EfCore_Custom_Filter_Tests()
    {
        _categoryRepository = GetRequiredService<IBasicRepository<Category, Guid>>();
    }

    [Fact]
    public async Task Should_Combine_Abp_And_Custom_QueryFilter_Test()
    {
        var categories = await _categoryRepository.GetListAsync();
        categories.Count.ShouldBe(1);
        categories[0].Name.ShouldBe("abp.cli");

        using (GetRequiredService<IDataFilter<ISoftDelete>>().Disable())
        {
            categories = await _categoryRepository.GetListAsync();
            categories.Count.ShouldBe(2);
            categories.ShouldContain(x => x.Name == "abp.cli" && x.IsDeleted == false);
            categories.ShouldContain(x => x.Name == "abp.core" && x.IsDeleted == true);
        }
    }
}
