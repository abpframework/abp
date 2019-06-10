using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services
{
    public interface IAsyncCrudAppService<TEntityDto, in TKey>
        : IAsyncCrudAppService<TEntityDto, TKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TKey, in TGetListInput>
        : IAsyncCrudAppService<TEntityDto, TKey, TGetListInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
        : IAsyncCrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IAsyncCrudAppService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface IAsyncCrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
        Task<TGetOutputDto> GetAsync(TKey id);

        Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);

        Task<TGetOutputDto> CreateAsync(TCreateInput input);

        Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input);

        Task DeleteAsync(TKey id);
    }
}
