using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

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

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetAllInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetAllInput, TCreateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetAllInput, TCreateInput, TCreateInput>
        where TGetAllInput : IPagedAndSortedResultRequest
        where TEntity : class, IEntity<TKey>
        where TEntityDto : IEntityDto<TKey>
        where TCreateInput : IEntityDto<TKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TKey, TGetAllInput, TCreateInput, TUpdateInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TKey, TGetAllInput, TCreateInput, TUpdateInput>,
        IAsyncCrudAppService<TEntityDto, TKey, TGetAllInput, TCreateInput, TUpdateInput>
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
            CheckGetPermission();

            var entity = await GetEntityByIdAsync(id);
            return MapToEntityDto(entity);
        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetListAsync(TGetAllInput input)
        {
            CheckGetAllPermission();

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
            CheckCreatePermission();

            var entity = MapToEntity(input);

            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public virtual async Task<TEntityDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(id);

            //TODO: Check if input has id different than given id and normalize if it's default value, throw ex otherwise

            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public virtual Task DeleteAsync(TKey id)
        {
            CheckDeletePermission();

            return Repository.DeleteAsync(id);
        }

        protected virtual Task<TEntity> GetEntityByIdAsync(TKey id)
        {
            return Repository.GetAsync(id);
        }
    }
}
