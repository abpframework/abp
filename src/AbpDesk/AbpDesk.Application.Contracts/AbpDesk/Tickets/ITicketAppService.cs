using System.Threading.Tasks;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpDesk.Tickets
{
    public interface ITicketAppService : IApplicationService
    {
        Task<ListResultDto<TicketDto>> GetAll(GetAllTicketsInput input);

        ListResultDto<TicketDto> GetAll2(GetAllTicketsInput input);
    }
}
