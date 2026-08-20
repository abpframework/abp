using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Application;
using Volo.Abp.Threading;
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

    [Fact]
    public async Task Should_Only_Select_The_Projected_Columns()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var repository = GetRequiredService<IReadOnlyRepository<Person, Guid>>();
            var projector = GetRequiredService<IQueryProjector<Person, PersonProjectionDto>>();

            var sql = projector.ProjectTo(await repository.GetQueryableAsync()).ToQueryString();

            sql.ShouldContain("\"Name\"");
            sql.ShouldNotContain("\"Birthday\"");
            sql.ShouldNotContain("\"ExtraProperties\"");

            //the data filters are still a part of the query
            sql.ShouldContain("Is_Deleted");
        });
    }

    [Fact]
    public async Task Should_Join_Another_Aggregate_While_Projecting()
    {
        var appService = GetRequiredService<PersonWithCityAppService>();

        var dto = await appService.GetAsync(TestDataBuilder.UserDouglasId);
        dto.CityName.ShouldBe("London");

        var result = await appService.GetListAsync(new PagedAndSortedResultRequestDto());
        result.Items.ShouldContain(x => x.CityName == "London");
    }

    [Fact]
    public async Task Should_Keep_The_Total_Count_While_Joining_Another_Aggregate()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            await GetRequiredService<IRepository<Person, Guid>>()
                .InsertAsync(new Person(Guid.NewGuid(), "PersonWithoutCity", 30), autoSave: true);
        });

        var result = await GetRequiredService<PersonWithCityAppService>()
            .GetListAsync(new PagedAndSortedResultRequestDto());

        result.Items.Count.ShouldBe((int)result.TotalCount);
        result.Items.ShouldContain(x => x.Name == "PersonWithoutCity" && x.CityName == null);
    }

    [Fact]
    public async Task Should_Execute_The_Projected_Query_From_The_Application_Service()
    {
        System.Collections.Concurrent.ConcurrentQueue<string> commands;
        using (SqlCommandCapture.Begin(out commands))
        {
            await GetRequiredService<PersonProjectionAppService>()
                .GetListAsync(new PagedAndSortedResultRequestDto());
        }

        //the application service must run the projection itself, not materialize the entities first
        var select = commands.Last(x => x.Contains("FROM \"People\"") && !x.Contains("COUNT"));

        select.ShouldContain("\"Name\"");
        select.ShouldNotContain("\"Birthday\"");
        select.ShouldNotContain("\"ExtraProperties\"");
    }

    [Fact]
    public async Task Should_Apply_The_Multi_Tenancy_Filter_While_Projecting()
    {
        var result = await GetRequiredService<PersonProjectionAppService>()
            .GetListAsync(new PagedAndSortedResultRequestDto());

        result.Items.ShouldNotContain(x => x.Name.StartsWith(TestDataBuilder.TenantId1.ToString()));
    }

    [Fact]
    public async Task Should_Use_The_Ambient_Cancellation_Token_While_Projecting()
    {
        var cancellationTokenProvider = GetRequiredService<ICancellationTokenProvider>();
        var appService = GetRequiredService<PersonProjectionAppService>();

        using (cancellationTokenProvider.Use(new CancellationToken(canceled: true)))
        {
            await Should.ThrowAsync<OperationCanceledException>(async () =>
                await appService.GetAsync(TestDataBuilder.UserDouglasId));

            await Should.ThrowAsync<OperationCanceledException>(async () =>
                await appService.GetListAsync(new PagedAndSortedResultRequestDto()));
        }
    }

    [Fact]
    public async Task Should_Use_The_Ambient_Cancellation_Token_Without_A_Projector()
    {
        var cancellationTokenProvider = GetRequiredService<ICancellationTokenProvider>();
        var appService = GetRequiredService<IPeopleAppService>();

        using (cancellationTokenProvider.Use(new CancellationToken(canceled: true)))
        {
            await Should.ThrowAsync<OperationCanceledException>(async () =>
                await appService.GetListAsync(new PagedAndSortedResultRequestDto()));
        }
    }
}
