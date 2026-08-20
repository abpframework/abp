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
    public void Should_Resolve_The_Projector_Independent_From_The_Class_Name()
    {
        ServiceProvider.GetService<IQueryProjector<Book, BookDto>>()
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
    public async Task Should_Use_The_Object_Mapper_If_No_Projector_Was_Registered()
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

    [Fact]
    public async Task Should_Use_The_Asynchronously_Created_Projection_Query()
    {
        var appService = GetRequiredService<BookAsyncProjectionAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookAsyncProjectionAppService.Marker);
        (await appService.GetListAsync(new PagedAndSortedResultRequestDto()))
            .Items[0].Name.ShouldEndWith(BookAsyncProjectionAppService.Marker);
    }

    [Fact]
    public async Task Should_Throw_EntityNotFoundException_From_An_Overridden_Projection_Query()
    {
        var appService = GetRequiredService<BookAsyncProjectionAppService>();

        await Should.ThrowAsync<EntityNotFoundException>(async () => await appService.GetAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Should_Throw_EntityNotFoundException_For_A_Value_Type_Dto()
    {
        var appService = GetRequiredService<BookStructAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldBe("Hitchhiker's Guide");

        await Should.ThrowAsync<EntityNotFoundException>(async () => await appService.GetAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Should_Not_Project_On_The_Create_And_Update_Paths()
    {
        var appService = GetRequiredService<BookAppService>();

        var created = await appService.CreateAsync(new BookDto { Name = "New Book" });
        created.Name.ShouldEndWith(BookObjectMapper.Marker);

        var updated = await appService.UpdateAsync(created.Id, new BookDto { Name = "Updated Book" });
        updated.Name.ShouldEndWith(BookObjectMapper.Marker);
    }

    [Fact]
    public async Task Should_Use_A_Different_Projector_For_The_Get_And_The_List()
    {
        var appService = GetRequiredService<BookDetailAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookDetailProjector.Marker);
        (await appService.GetListAsync(new PagedAndSortedResultRequestDto()))
            .Items[0].Name.ShouldEndWith(BookProjector.Marker);
    }

    [Fact]
    public async Task Should_Apply_Paging_And_Sorting_Before_Projecting()
    {
        var repository = GetRequiredService<IRepository<Book, Guid>>();
        await repository.InsertAsync(new Book(Guid.NewGuid(), "A Book", 1));
        await repository.InsertAsync(new Book(Guid.NewGuid(), "Z Book", 2));

        var appService = GetRequiredService<BookAppService>();

        var firstPage = await appService.GetListAsync(
            new PagedAndSortedResultRequestDto { MaxResultCount = 1, Sorting = "Name" });

        firstPage.TotalCount.ShouldBe(3);
        firstPage.Items.Count.ShouldBe(1);
        firstPage.Items[0].Name.ShouldBe("A Book" + BookProjector.Marker);

        var secondPage = await appService.GetListAsync(
            new PagedAndSortedResultRequestDto { MaxResultCount = 1, SkipCount = 1, Sorting = "Name" });

        secondPage.Items[0].Name.ShouldBe("Hitchhiker's Guide" + BookProjector.Marker);
    }

    [Fact]
    public async Task Should_Project_A_Single_Entity_From_An_AbstractKey_Application_Service()
    {
        var appService = GetRequiredService<BookAbstractKeyProjectingAppService>();

        (await appService.GetAsync(_bookId)).Name.ShouldEndWith(BookProjector.Marker);
    }
}
