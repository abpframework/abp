using System.Collections.Generic;
using AbpDesk.Tickets.Dtos;

namespace AbpDesk.Models.Tickets
{
    public class IndexViewModel
    {
        public IReadOnlyList<TicketDto> Tickets { get; set; }
    }
}
