using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

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
        where TCreateInput : IEntityDto<TKey>
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

        protected override TEntityDto MapToGetListOutputDto(TEntity entity)
        {
            return MapToGetOutputDto(entity);
        }
    }

    public abstract class CrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
       : CrudAppServiceBase<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        ICrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity<TKey>
           where TGetOutputDto : IEntityDto<TKey>
           where TGetListOutputDto : IEntityDto<TKey>
    {
        protected CrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }

        public virtual TGetOutputDto Get(TKey id)
        {
            CheckGetPolicy();

            var entity = GetEntityById(id);
            return MapToGetOutputDto(entity);
        }

        public virtual PagedResultDto<TGetListOutputDto> GetList(TGetListInput input)
        {
            CheckGetListPolicy();

            var query = CreateFilteredQuery(input);

            var totalCount = query.Count();

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = query.ToList();

            return new PagedResultDto<TGetListOutputDto>(
                totalCount,
                entities.Select(MapToGetListOutputDto).ToList()
            );
        }

        public virtual TGetOutputDto Create(TCreateInput input)
        {
            CheckCreatePolicy();

            var entity = MapToEntity(input);

            if (entity is IMultiTenant && !HasTenantIdProperty(entity))
            {
                TryToSetTenantId(entity);
            }

            Repository.Insert(entity, autoSave: true);

            return MapToGetOutputDto(entity);
        }

        public virtual TGetOutputDto Update(TKey id, TUpdateInput input)
        {
            CheckUpdatePolicy();

            var entity = GetEntityById(id);
            MapToEntity(input, entity);
            Repository.Update(entity, autoSave: true);

            return MapToGetOutputDto(entity);
        }

        public virtual void Delete(TKey id)
        {
            CheckDeletePolicy();

            Repository.Delete(id);
        }

        protected virtual TEntity GetEntityById(TKey id)
        {
            return Repository.Get(id);
        }

        protected virtual void CheckGetPolicy()
        {
            CheckPolicy(GetPolicyName);
        }

        protected virtual void CheckGetListPolicy()
        {
            CheckPolicy(GetListPolicyName);
        }

        protected virtual void CheckCreatePolicy()
        {
            CheckPolicy(CreatePolicyName);
        }

        protected virtual void CheckUpdatePolicy()
        {
            CheckPolicy(UpdatePolicyName);
        }

        protected virtual void CheckDeletePolicy()
        {
            CheckPolicy(DeletePolicyName);
        }
    }
}
