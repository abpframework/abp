using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Application.Services
{
    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TKey>
        : AsyncCrudAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetListInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        IAsyncCrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity<TKey>
           where TEntityDto : IEntityDto<TKey>
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        protected AsyncCrudAppService(IRepository<TEntity, TKey> repository)
            :base(repository)
        {
            AsyncQueryableExecuter = DefaultAsyncQueryableExecuter.Instance;
        }

        public virtual async Task<TEntityDto> GetAsync(TKey id)
        {
            await CheckGetPolicyAsync();

            var entity = await GetEntityByIdAsync(id);
            return MapToEntityDto(entity);
        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetListAsync(TGetListInput input)
        {
            await CheckGetListPolicyAsync();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePolicyAsync();

            var entity = MapToEntity(input);
            
            if(entity is IMultiTenant)
            {
                TryToSetTenantId(entity);
            }
            
            await Repository.InsertAsync(entity, autoSave: true);

            return MapToEntityDto(entity);
        }

        public virtual async Task<TEntityDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);
            //TODO: Check if input has id different than given id and normalize if it's default value, throw ex otherwise
            MapToEntity(input, entity);
            await Repository.UpdateAsync(entity, autoSave: true);

            return MapToEntityDto(entity);
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            await CheckDeletePolicyAsync();

            await Repository.DeleteAsync(id);
        }

        protected virtual Task<TEntity> GetEntityByIdAsync(TKey id)
        {
            return Repository.GetAsync(id);
        }

        protected virtual async Task CheckGetPolicyAsync()
        {
            await CheckPolicyAsync(GetPolicyName);
        }

        protected virtual async Task CheckGetListPolicyAsync()
        {
            await CheckPolicyAsync(GetListPolicyName);
        }

        protected virtual async Task CheckCreatePolicyAsync()
        {
            await CheckPolicyAsync(CreatePolicyName);
        }

        protected virtual async Task CheckUpdatePolicyAsync()
        {
            await CheckPolicyAsync(UpdatePolicyName);
        }

        protected virtual async Task CheckDeletePolicyAsync()
        {
            await CheckPolicyAsync(DeletePolicyName);
        }
    }
}
