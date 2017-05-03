using System.Linq;
using System.Threading.Tasks;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Services.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using Volo.Abp.Linq.Extensions;
using Volo.ExtensionMethods;

namespace AbpDesk.Tickets
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IQueryableRepository<Ticket, int> _ticketRepository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        public TicketAppService(
            IQueryableRepository<Ticket, int> ticketRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter
            )
        {
            _ticketRepository = ticketRepository;
            _asyncQueryableExecuter = asyncQueryableExecuter;
        }

        public async Task<ListResultDto<TicketDto>> GetAll(GetAllTicketsInput input)
        {
            var tickets = await _asyncQueryableExecuter.ToListAsync(_ticketRepository
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    t => t.Title.Contains(input.Filter) || t.Body.Contains(input.Filter)
                )
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Body = t.Body
                }));

            return new ListResultDto<TicketDto>(tickets);
        }
    }
}
