using System;
using AbpDesk.Tickets;
using AbpDesk.Tickets.Dtos;
using Volo.Abp.Threading;
using Volo.Abp.DependencyInjection;

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
            Console.WriteLine();
            Console.WriteLine("List of tickets:");

            var result = AsyncHelper.RunSync(() => _ticketAppService.GetAll(new GetAllTicketsInput()));

            foreach (var ticket in result.Items)
            {
                Console.WriteLine(ticket);
            }
        }
    }

    //Uses TicketAppService instead of ITicketAppService.
    public class TicketLister2 : ITransientDependency
    {
        private readonly TicketAppService _ticketAppService;

        public TicketLister2(TicketAppService ticketAppService)
        {
            _ticketAppService = ticketAppService;
        }

        public void List()
        {
            Console.WriteLine();
            Console.WriteLine("List of tickets:");

            var result = AsyncHelper.RunSync(() => _ticketAppService.GetAll(new GetAllTicketsInput()));

            foreach (var ticket in result.Items)
            {
                Console.WriteLine(ticket);
            }
        }
    }
}