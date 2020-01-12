using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services
{
    public interface ICrudAppService<TEntityDto, in TKey>
        : ICrudAppService<TEntityDto, TKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput>
        : ICrudAppService<TEntityDto, TKey, TGetListInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
        : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface ICrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IBaseCrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
        Task<TGetOutputDto> GetAsync(TKey id);
    }
}
