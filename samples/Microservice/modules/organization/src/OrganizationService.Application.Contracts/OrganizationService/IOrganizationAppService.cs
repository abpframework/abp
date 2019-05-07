using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace OrganizationService
{
    public interface IOrganizationAppService :
            IAsyncCrudAppService< //Defines CRUD methods
                OrganizationDto, //Used to show books
                Guid, //Primary key of the book entity
                OrganizationPagedRequestDto, //Used for paging/sorting on getting a list of books
                CreateUpdateAbpOrganizationDto, //Used to create a new book
                CreateUpdateAbpOrganizationDto> //Used to update a book
    {
        List<ViewTree> GetViewTrees(Guid? guid);

        Task<List<ViewTree>> GetUserViewTrees(Guid? userId);

        Task SetOrganizationsAsync(SetUserOrgaizationDto orgaizationDto);
    }
}