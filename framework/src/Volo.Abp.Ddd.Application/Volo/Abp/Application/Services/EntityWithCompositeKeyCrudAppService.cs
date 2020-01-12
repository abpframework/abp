using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services
{
    public abstract class EntityWithCompositeKeyCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
       : BaseCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        IEntityWithCompositeKeyCrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity
        where TGetOutputDto : IEntityDto
        where TGetListOutputDto : IEntityDto
    {
        protected EntityWithCompositeKeyCrudAppService(IRepository<TEntity> repository)
            : base(repository) { }

        public virtual async Task<TGetOutputDto> GetFindAsync(TKey key)
        {
            await CheckGetPolicyAsync().ConfigureAwait(false);

            var entity = await GetEntityByIdAsync(key).ConfigureAwait(false);

            return MapToDto<TGetOutputDto>(entity);

        }
        [RemoteService(false)]//disable for http api
        public override Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            return this.UpdateAsync(input);
        }

        public virtual async Task<TGetOutputDto> UpdateAsync(TUpdateInput input)
        {
            var keys = ObjectMapper.Map<TUpdateInput, TKey>(input);

            return await base.UpdateAsync(keys, input);

        }

        public override async Task DeleteAsync(TKey input)
        {
            await base.DeleteAsync(input);
        }


    }

}
