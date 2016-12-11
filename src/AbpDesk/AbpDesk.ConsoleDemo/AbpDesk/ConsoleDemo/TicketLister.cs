using System;
using AbpDesk.Tickets;
using AbpDesk.Tickets.Dtos;
using Volo.DependencyInjection;

namespace AbpDesk.ConsoleDemo
{
    public class TicketLister : ITransientDependency
    {
        private readonly ITicketAppService _ticketAppService;

        public TicketLister(ITicketAppService ticketAppService)
        {
            _ticketAppService = ticketAppService;
        }

        public void List()
        {
            var result = _ticketAppService.GetAll(new GetAllTicketsInput());

            foreach (var ticket in result.Items)
            {
                Console.WriteLine(ticket);
            }
        }
    }
}