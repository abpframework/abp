using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace Volo.Abp.Application.Services.QueryProjection;

public class QueryProjection_Tests : AbpDddApplicationTestBase
{
    private readonly Guid _bookId = Guid.NewGuid();

    public QueryProjection_Tests()
    {
        var repository = GetRequiredService<IRepository<Book, Guid>>();
        repository.InsertAsync(new Book(_bookId, "Hitchhiker's Guide", 42)).GetAwaiter().GetResult();
    }

    [Fact]
    public void Should_Resolve_Projection_Mapper_Independent_From_The_Class_Name()
    {
        ServiceProvider.GetService<IQueryableMapper<Book, BookDto>>()
            .ShouldBeOfType<BookProjector>();
    }

    [Fact]
    public async Task Should_Project_On_The_Query_For_CrudAppService()
    {
        var appService = GetRequiredService<BookAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookProjector.Marker);
        (await appService.GetListAsync(new PagedAndSortedResultRequestDto()))
            .Items[0].Name.ShouldEndWith(BookProjector.Marker);
    }

    [Fact]
    public async Task Should_Project_On_The_Query_For_ReadOnlyAppService()
    {
        var appService = GetRequiredService<BookReadOnlyAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookProjector.Marker);
        (await appService.GetListAsync(new PagedAndSortedResultRequestDto()))
            .Items[0].Name.ShouldEndWith(BookProjector.Marker);
    }

    [Fact]
    public async Task Should_Throw_EntityNotFoundException_If_The_Projected_Entity_Does_Not_Exist()
    {
        var appService = GetRequiredService<BookAppService>();

        await Should.ThrowAsync<EntityNotFoundException>(async () => await appService.GetAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Should_Use_The_Object_Mapper_If_No_Projection_Mapper_Was_Registered()
    {
        var appService = GetRequiredService<BookLiteAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookObjectMapper.Marker);
        (await appService.GetListAsync(new PagedAndSortedResultRequestDto()))
            .Items[0].Name.ShouldEndWith(BookObjectMapper.Marker);
    }

    [Fact]
    public async Task Should_Use_The_Object_Mapper_If_The_Projection_Was_Disabled()
    {
        var appService = GetRequiredService<BookWithoutProjectionAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookObjectMapper.Marker);
        (await appService.GetListAsync(new PagedAndSortedResultRequestDto()))
            .Items[0].Name.ShouldEndWith(BookObjectMapper.Marker);
    }

    [Fact]
    public async Task Should_Not_Use_The_Entity_Based_Overrides_While_Projecting()
    {
        var appService = GetRequiredService<BookCustomizedAppService>();

        var dto = await appService.GetAsync(_bookId);

        dto.Name.ShouldEndWith(BookProjector.Marker);
        dto.Name.ShouldNotContain(BookCustomizedAppService.Marker);
    }

    [Fact]
    public async Task Should_Use_The_Object_Mapper_If_The_Entity_Query_Can_Not_Be_Created()
    {
        var appService = GetRequiredService<BookAbstractKeyAppService>();

        //GetAsync has no query to project, it falls back to GetEntityByIdAsync
        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookObjectMapper.Marker);

        //GetListAsync always has a query, so it is still projected
        (await appService.GetListAsync(new PagedAndSortedResultRequestDto()))
            .Items[0].Name.ShouldEndWith(BookProjector.Marker);
    }
}
