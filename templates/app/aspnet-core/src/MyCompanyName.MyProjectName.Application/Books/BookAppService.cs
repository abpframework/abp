using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MyCompanyName.MyProjectName.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Domain.Repositories;

namespace MyCompanyName.MyProjectName.Books;

[Authorize(MyProjectNamePermissions.Books.Default)]
public class BookAppService : CrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateBookDto>, IBookAppService
{
    private readonly ResourcePermissionPopulator _resourcePermissionPopulator;

    public BookAppService(IRepository<Book, Guid> repository, ResourcePermissionPopulator resourcePermissionPopulator) : base(repository)
    {
        _resourcePermissionPopulator = resourcePermissionPopulator;
        GetPolicyName = MyProjectNamePermissions.Books.Default;
        GetListPolicyName = MyProjectNamePermissions.Books.Default;
        CreatePolicyName = MyProjectNamePermissions.Books.Create;
        UpdatePolicyName = MyProjectNamePermissions.Books.Edit;
        DeletePolicyName = MyProjectNamePermissions.Books.Delete;
    }

    public override async Task<BookDto> GetAsync(Guid id)
    {
        var book = await Repository.GetAsync(id);
        var bookDto = ObjectMapper.Map<Book, BookDto>(book);
        await _resourcePermissionPopulator.PopulateAsync(bookDto, typeof(Book).FullName!);

        return bookDto;
    }

    public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var result = await base.GetListAsync(input);
        await _resourcePermissionPopulator.PopulateAsync(result.Items.ToList(), typeof(Book).FullName!);
        return result;
    }

    public override async Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input)
    {
        await CheckUpdatePolicyAsync();

        var entity = await GetEntityByIdAsync(id);

        // PublishDate is always allowed to be updated
        entity.PublishDate = input.PublishDate;

        // Field-level permission checks
        var x = await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangeName);
        if (await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangeName))
        {
            entity.Name = input.Name;
        }

        var y = await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangeType);
        if (await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangeType))
        {
            entity.BookType = input.BookType;
        }


        var z = await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangeAuthor);
        if (await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangeAuthor))
        {
            entity.Author = input.Author;
        }

        var w = await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangePrice);
        if (await AuthorizationService.IsGrantedAsync(entity, MyProjectNamePermissions.Books.Resources.ChangePrice))
        {
            entity.Price = input.Price;
        }

        await Repository.UpdateAsync(entity, autoSave: true);

        return await MapToGetOutputDtoAsync(entity);
    }
}
