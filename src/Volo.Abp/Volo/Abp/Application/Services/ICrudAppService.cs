using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services
{
    public interface ICrudAppService<TEntityDto>
        : ICrudAppService<TEntityDto, Guid>
        where TEntityDto : IEntityDto<Guid>
    {

    }

    public interface ICrudAppService<TEntityDto, in TPrimaryKey>
        : ICrudAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, in TPrimaryKey, in TGetListInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetListInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, in TPrimaryKey, in TGetListInput, in TCreateInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, in TPrimaryKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        TEntityDto Get(TPrimaryKey id);

        PagedResultDto<TEntityDto> GetAll(TGetListInput input);

        TEntityDto Create(TCreateInput input);

        TEntityDto Update(TPrimaryKey id, TUpdateInput input);

        void Delete(TPrimaryKey id);
    }
}
