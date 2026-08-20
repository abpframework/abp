using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class QueryProjection_Tests : EntityFrameworkCoreTestBase
{
    private readonly PersonProjectionAppService _personProjectionAppService;

    public QueryProjection_Tests()
    {
        _personProjectionAppService = GetRequiredService<PersonProjectionAppService>();
    }

    [Fact]
    public async Task Should_Get_A_Projected_Entity()
    {
        var dto = await _personProjectionAppService.GetAsync(TestDataBuilder.UserDouglasId);

        dto.Id.ShouldBe(TestDataBuilder.UserDouglasId);
        dto.Name.ShouldBe("Douglas");
    }

    [Fact]
    public async Task Should_Throw_EntityNotFoundException_For_A_Missing_Entity()
    {
        (await Should.ThrowAsync<EntityNotFoundException<Person>>(
            async () => await _personProjectionAppService.GetAsync(Guid.NewGuid()))
        ).EntityType.ShouldBe(typeof(Person));
    }

    [Fact]
    public async Task Should_Get_A_Projected_List()
    {
        var result = await _personProjectionAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.Count.ShouldBe((int)result.TotalCount);
        result.Items.ShouldContain(x => x.Name == "Douglas");
    }

    [Fact]
    public async Task Should_Get_A_Projected_Entity_With_A_Non_Guid_Key()
    {
        var appService = GetRequiredService<EntityWithIntPkProjectionAppService>();
        var entity = await WithUnitOfWorkAsync(
            async () => await GetRequiredService<IRepository<EntityWithIntPk, int>>().FirstAsync());

        (await appService.GetAsync(entity.Id)).Name.ShouldBe(entity.Name);
    }

    [Fact]
    public async Task Should_Apply_The_Soft_Delete_Filter_While_Projecting()
    {
        var result = await _personProjectionAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        result.Items.ShouldNotContain(x => x.Id == TestDataBuilder.UserJohnDeletedId);
    }
}
