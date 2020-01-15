using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services
{
    public abstract class EntityWithCompositeKeyCrudAppService<TEntity, TGetOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
       : BaseCrudAppService<TEntity, TGetOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        IEntityWithCompositeKeyCrudAppService<TGetOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
           where TEntity : class, IEntity
        where TGetOutputDto : IEntityDto
    {
        protected EntityWithCompositeKeyCrudAppService(IRepository<TEntity> repository)
            : base(repository) { }

        public virtual async Task<TGetOutputDto> GetFindAsync(TKey key)
        {
            await CheckGetPolicyAsync().ConfigureAwait(false);

            var entity = await GetEntityByIdAsync(key).ConfigureAwait(false);

            return MapToDto<TGetOutputDto>(entity);

        }
        [RemoteService(false)]//disable for http api,because of id is a object type,can't bind to url path
        public override async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            return await base.UpdateAsync(id, input).ConfigureAwait(false);
        }

        public virtual async Task<TGetOutputDto> UpdateAsync(TUpdateInput input)
        {
            var keys = ObjectMapper.Map<TUpdateInput, TKey>(input);

            return await this.UpdateAsync(keys, input).ConfigureAwait(false);

        }

        public override async Task DeleteAsync(TKey input)//Tkey is a object type,so change name from id to input 
        {
            await base.DeleteAsync(input).ConfigureAwait(false);
        }


    }

}
