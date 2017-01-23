using System.Threading.Tasks;
using AbpDesk.Tickets;
using AbpDesk.Tickets.Dtos;
using AbpDesk.Web.Mvc.Models.Tickets;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpDesk.Web.Mvc.Controllers
{
    public class TicketsController : AbpController
    {
        private readonly ITicketAppService _ticketAppService;

        public TicketsController(ITicketAppService ticketAppService)
        {
            _ticketAppService = ticketAppService;
        }

        public async Task<IActionResult> Index(GetAllTicketsInput input)
        {
            var result = await _ticketAppService.GetAll(input);

            var model = new IndexViewModel
            {
                Tickets = result.Items
            };

            return View(model);
        }
    }
}
