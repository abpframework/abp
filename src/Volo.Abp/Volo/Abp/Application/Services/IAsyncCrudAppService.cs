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

    public interface IAsyncCrudAppService<TEntityDto, in TPrimaryKey, in TGetListInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetListInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TPrimaryKey, in TGetListInput, in TCreateInput>
        : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TPrimaryKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> GetAsync(TPrimaryKey id);

        Task<PagedResultDto<TEntityDto>> GetListAsync(TGetListInput input);

        Task<TEntityDto> CreateAsync(TCreateInput input);

        Task<TEntityDto> UpdateAsync(TPrimaryKey id, TUpdateInput input);

        Task DeleteAsync(TPrimaryKey id);
    }
}
