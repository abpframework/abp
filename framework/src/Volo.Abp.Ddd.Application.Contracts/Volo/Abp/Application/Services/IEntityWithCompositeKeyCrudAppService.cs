using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services
{
    public interface IEntityWithCompositeKeyCrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IBaseCrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TGetOutputDto : IEntityDto
        where TGetListOutputDto : IEntityDto
    {
        Task<TGetOutputDto> GetFindAsync(TKey key);

        Task<TGetOutputDto> UpdateAsync(TUpdateInput input);
    }
}
