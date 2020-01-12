using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Application.Services
{
    public interface IBaseCrudAppService<TGetOutputDto, TGetListOutputDto,in TKey,in TGetListInput,in TCreateInput,in TUpdateInput>
        : IApplicationService
        where TGetOutputDto : IEntityDto
        where TGetListOutputDto : IEntityDto
    {
        Task<PagedResultDto<TGetOutputDto>> GetListWithDetailsAsync(TGetListInput input);
        Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);
        Task<TGetOutputDto> CreateAsync(TCreateInput input);
        Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input);
        Task DeleteAsync(TKey id);
    }
}
