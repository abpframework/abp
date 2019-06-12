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
        : ICrudAppService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TKey>
    {

    }

    public interface ICrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
        TGetOutputDto Get(TKey id);

        PagedResultDto<TGetListOutputDto> GetList(TGetListInput input);

        TGetOutputDto Create(TCreateInput input);

        TGetOutputDto Update(TKey id, TUpdateInput input);

        void Delete(TKey id);
    }
}
