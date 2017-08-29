using Volo.Abp.Application.Dtos;

namespace AbpDesk.Tickets.Dtos
{
    public class TicketDto : EntityDto<int>
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, Title = {Title}, Body = {Body}";
        }
    }
}