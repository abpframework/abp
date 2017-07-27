using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Services.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using Volo.Abp.Linq.Extensions;
using Volo.ExtensionMethods;

namespace AbpDesk.Tickets
{
    public class TicketAppService : ApplicationService, ITicketAppService
    {
        private readonly IQueryableRepository<Ticket, int> _ticketRepository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        public TicketAppService(
            IQueryableRepository<Ticket, int> ticketRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter)
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
            );

            return new ListResultDto<TicketDto>(
                ObjectMapper.Map<List<Ticket>, List<TicketDto>>(tickets)
            );
        }

        public ListResultDto<TicketDto> GetAll2(GetAllTicketsInput input)
        {
            var tickets = _ticketRepository
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    t => t.Title.Contains(input.Filter) || t.Body.Contains(input.Filter)
                )
                .ToList();

            return new ListResultDto<TicketDto>(
                ObjectMapper.Map<List<Ticket>, List<TicketDto>>(tickets)
            );
        }
    }
}
