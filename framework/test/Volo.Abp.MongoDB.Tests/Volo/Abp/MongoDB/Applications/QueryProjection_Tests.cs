using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.MongoDB.Applications;

[Collection(MongoTestCollection.Name)]
public class QueryProjection_Tests : MongoDbTestBase
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
        await Should.ThrowAsync<EntityNotFoundException<Person>>(
            async () => await _personProjectionAppService.GetAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Should_Get_A_Projected_List()
    {
        var result = await _personProjectionAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(x => x.Name == "Douglas");
    }
}
