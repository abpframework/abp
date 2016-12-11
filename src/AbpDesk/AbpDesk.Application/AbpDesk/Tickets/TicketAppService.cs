using System.Linq;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Services.Dtos;
using Volo.Abp.Domain.Repositories;

namespace AbpDesk.Tickets
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IRepository<Ticket, int> _ticketRepository;

        public TicketAppService(IRepository<Ticket, int> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public ListResultDto<TicketDto> GetAll()
        {
            var tickets = _ticketRepository
                .GetList()
                .Select(t => new TicketDto { Id = t.Id, Title = t.Title, Body = t.Body })
                .ToList();

            return new ListResultDto<TicketDto>(tickets);
        }
    }
}
