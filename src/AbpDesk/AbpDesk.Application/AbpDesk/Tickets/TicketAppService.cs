using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace AbpDesk.Tickets
{
    public class TicketAppService : ApplicationService, ITicketAppService
    {
        private readonly IRepository<Ticket, int> _ticketRepository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        public TicketAppService(
            IRepository<Ticket, int> ticketRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter)
        {
            _ticketRepository = ticketRepository;
            _asyncQueryableExecuter = asyncQueryableExecuter;
        }

        //TODO: No need to virtual once we implement UOW filter for AspNet Core!
        public virtual async Task<ListResultDto<TicketDto>> GetAll(GetAllTicketsInput input)
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
