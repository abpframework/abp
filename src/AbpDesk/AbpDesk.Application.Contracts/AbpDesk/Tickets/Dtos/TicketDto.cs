using Volo.Abp.Application.Services.Dtos;

namespace AbpDesk.Tickets.Dtos
{
    public class TicketDto : EntityDto<int>
    {
        public string Title { get; set; }

        public string Body { get; set; }
    }
}