using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace OrganizationService
{
    public interface IOrganizationAppService :
            IAsyncCrudAppService< //Defines CRUD methods
                AbpOrganizationDto, //Used to show books
                Guid, //Primary key of the book entity
                AbpOrganizationPagedRequestDto, //Used for paging/sorting on getting a list of books
                CreateUpdateAbpOrganizationDto, //Used to create a new book
                CreateUpdateAbpOrganizationDto> //Used to update a book
    {
        List<ViewTree> GetViewTrees(Guid? guid);
    }
}