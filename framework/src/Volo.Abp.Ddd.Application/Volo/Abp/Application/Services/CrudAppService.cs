using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

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
       : CrudAppServiceBase<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity<TKey>
           where TEntityDto : IEntityDto<TKey>
    {
        protected CrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }

        public virtual TEntityDto Get(TKey id)
        {
            CheckGetPolicy();

            var entity = GetEntityById(id);
            return MapToEntityDto(entity);
        }

        public virtual PagedResultDto<TEntityDto> GetList(TGetListInput input)
        {
            CheckGetListPolicy();

            var query = CreateFilteredQuery(input);

            var totalCount = query.Count();

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = query.ToList();

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        public virtual TEntityDto Create(TCreateInput input)
        {
            CheckCreatePolicy();

            var entity = MapToEntity(input);

            Repository.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public virtual TEntityDto Update(TKey id, TUpdateInput input)
        {
            CheckUpdatePolicy();

            var entity = GetEntityById(id);
            
            MapToEntity(input, entity);
            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
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
