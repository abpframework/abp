using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services
{
    public interface IAsyncCrudAppService<TEntityDto>
        : IAsyncCrudAppService<TEntityDto, Guid>
        where TEntityDto : IEntityDto<Guid>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TPrimaryKey>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TPrimaryKey, in TGetAllInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TPrimaryKey, in TGetAllInput, in TCreateInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> Get(TPrimaryKey id);

        Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input);

        Task<TEntityDto> Create(TCreateInput input);

        Task<TEntityDto> Update(TPrimaryKey id, TUpdateInput input);

        Task Delete(TPrimaryKey id);
    }
}
