using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BaseManagement
{
        public interface IBaseTypeAppService :
            IAsyncCrudAppService< //Defines CRUD methods
                BaseTypeDto, //Used to show books
                Guid, //Primary key of the book entity
                PagedAndSortedResultRequestDto, //Used for paging/sorting on getting a list of books
                CreateBaseTypeDto, //Used to create a new book
                UpdateBaseTypeDto> //Used to update a book
        {

        }
    }