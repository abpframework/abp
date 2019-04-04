using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BaseManagement
{
    public interface IBaseTypeAppService :
            IAsyncCrudAppService< //Defines CRUD methods
                BaseTypeDto, //Used to show books
                Guid, //Primary key of the book entity
                BaseTypePagedRequestDto, //Used for paging/sorting on getting a list of books
                CreateUpdateBaseTypeDto, //Used to create a new book
                CreateUpdateBaseTypeDto> //Used to update a book
    {
        List<ViewTree> GetViewTrees(Guid? guid);
    }
}