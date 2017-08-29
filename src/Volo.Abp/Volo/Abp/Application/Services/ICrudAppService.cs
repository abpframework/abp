using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Abp.Application.Services
{
    public interface ICrudAppService<TEntityDto>
        : ICrudAppService<TEntityDto, Guid>
        where TEntityDto : IEntityDto<Guid>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey>
        : ICrudAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
    : ICrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey> 
        where TGetInput : IEntityDto<TPrimaryKey>
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput>
        : IApplicationService
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        TEntityDto Get(TGetInput input);

        PagedResultDto<TEntityDto> GetAll(TGetAllInput input);

        TEntityDto Create(TCreateInput input);

        TEntityDto Update(TUpdateInput input);

        void Delete(TDeleteInput input);
    }
}
