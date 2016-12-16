using System.Linq;
using System.Threading.Tasks;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Application.Services.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq.Extensions;
using Volo.Abp.Uow;
using Volo.ExtensionMethods;

namespace AbpDesk.Tickets
{
    public class TicketAppService : ITicketAppService
    {
        private readonly IQueryableRepository<Ticket, int> _ticketRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TicketAppService(
            IQueryableRepository<Ticket, int> ticketRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _ticketRepository = ticketRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ListResultDto<TicketDto>> GetAll(GetAllTicketsInput input)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var tickets = _ticketRepository
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        t => t.Title.Contains(input.Filter) || t.Body.Contains(input.Filter)
                    ).Select(t => new TicketDto { Id = t.Id, Title = t.Title, Body = t.Body })
                    .ToList();

                await unitOfWork.CompleteAsync();

                return new ListResultDto<TicketDto>(tickets);
            }
        }
    }
}
