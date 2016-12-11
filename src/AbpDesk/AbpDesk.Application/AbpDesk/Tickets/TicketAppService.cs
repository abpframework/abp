using System.Linq;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Services.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq.Extensions;
using Volo.ExtensionMethods;

namespace AbpDesk.Tickets
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IQueryableRepository<Ticket, int> _ticketRepository;

        public TicketAppService(IQueryableRepository<Ticket, int> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public ListResultDto<TicketDto> GetAll(GetAllTicketsInput input)
        {
            var tickets = _ticketRepository
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    t => t.Title.Contains(input.Filter) || t.Body.Contains(input.Filter)
                ).Select(t => new TicketDto { Id = t.Id, Title = t.Title, Body = t.Body })
                .ToList();

            return new ListResultDto<TicketDto>(tickets);
        }
    }
}
