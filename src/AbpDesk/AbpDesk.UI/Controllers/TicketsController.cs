using AbpDesk.Models.Tickets;
using AbpDesk.Tickets;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpDesk.Controllers
{
    public class TicketsController : AbpController
    {
        private readonly ITicketAppService _ticketAppService;

        public TicketsController(ITicketAppService ticketAppService)
        {
            _ticketAppService = ticketAppService;
        }

        public IActionResult Index()
        {
            var result = _ticketAppService.GetAll();

            var model = new IndexViewModel
            {
                Tickets = result.Items
            };

            return View(model);
        }
    }
}
