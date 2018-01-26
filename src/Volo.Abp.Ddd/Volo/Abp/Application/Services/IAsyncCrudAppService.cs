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
        : IApplicationService
        where TEntityDto : IEntityDto<TKey>
    {
        Task<TEntityDto> GetAsync(TKey id);

        Task<PagedResultDto<TEntityDto>> GetListAsync(TGetListInput input);

        Task<TEntityDto> CreateAsync(TCreateInput input);

        Task<TEntityDto> UpdateAsync(TKey id, TUpdateInput input);

        Task DeleteAsync(TKey id);
    }
}
