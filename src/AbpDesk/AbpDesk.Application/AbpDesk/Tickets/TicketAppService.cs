using System.Collections.Generic;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Services.Dtos;

namespace AbpDesk.Tickets
{
    public class TicketAppService : ITicketAppService
    {
        public ListResultDto<TicketDto> GetAll()
        {
            return new ListResultDto<TicketDto>(new List<TicketDto>
            {
                new TicketDto {Id = 1, Title = "Ticket 1 Title", Body = "Ticket 1 Body" }
            });
        }
    }
}
