using System.Collections.Generic;
using AbpDesk.Tickets.Dtos;

namespace AbpDesk.Web.Mvc.Models.Tickets
{
    public class IndexViewModel
    {
        public IReadOnlyList<TicketDto> Tickets { get; set; }
    }
}
