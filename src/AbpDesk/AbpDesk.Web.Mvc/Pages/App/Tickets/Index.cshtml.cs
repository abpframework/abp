using System.Collections.Generic;
using System.Threading.Tasks;
using AbpDesk.Tickets;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace AbpDesk.Web.Mvc.Pages.App.Tickets
{
    public class IndexModel : AbpPageModel
    {
        public IReadOnlyList<TicketDto> Tickets { get; set; }
        
        private readonly ITicketAppService _ticketAppService;

        public IndexModel(ITicketAppService ticketAppService)
        {
            _ticketAppService = ticketAppService;
        }

        public async Task OnGetAsync(GetAllTicketsInput input)
        {
            var uow = CurrentUnitOfWork;
            var result = await _ticketAppService.GetAll(input);
            Tickets = result.Items;
        }
    }
}
