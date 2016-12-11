using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Services.Dtos;

namespace AbpDesk.Tickets
{
    public interface ITicketAppService : IApplicationService
    {
        ListResultDto<TicketDto> GetAll(GetAllTicketsInput input);
    }
}
