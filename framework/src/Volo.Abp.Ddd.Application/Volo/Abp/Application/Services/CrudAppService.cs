using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services
{
    public abstract class CrudAppService<TEntity, TEntityDto, TKey>
        : CrudAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected CrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class CrudAppService<TEntity, TEntityDto, TKey, TGetListInput>
        : CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected CrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
        : CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected CrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class CrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        : CrudAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected CrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class CrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
       : BaseCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        ICrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity<TKey>
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
    {
        protected new IRepository<TEntity, TKey> Repository { get; }

        protected CrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {
            this.Repository = repository;
        }

        public virtual async Task<TGetOutputDto> GetAsync(TKey id)
        {
            await CheckGetPolicyAsync().ConfigureAwait(false);

            var entity = await GetEntityByIdAsync(id).ConfigureAwait(false);

            return MapToDto<TGetOutputDto>(entity);
        }
    }

}
