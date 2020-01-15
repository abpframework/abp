using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services
{
    public interface IEntityWithCompositeKeyCrudAppService<TGetOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IBaseCrudAppService<TGetOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TGetOutputDto : IEntityDto
    {
        Task<TGetOutputDto> GetFindAsync(TKey key);

        Task<TGetOutputDto> UpdateAsync(TUpdateInput input);
    }
}
