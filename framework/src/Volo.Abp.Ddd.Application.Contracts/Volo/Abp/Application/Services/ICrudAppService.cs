using System;
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
        : IBaseCrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TKey>
    {
        Task<TEntityDto> GetAsync(TKey id);
    }

    [Obsolete("This class is obsolete.Use ICrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput> instead.",false)]
    public interface ICrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TGetOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
    }
}
