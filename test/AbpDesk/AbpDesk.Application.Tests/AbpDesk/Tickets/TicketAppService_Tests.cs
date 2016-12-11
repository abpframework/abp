using AbpDesk.Tickets.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace AbpDesk.Tickets
{
    public class TicketAppService_Tests : AbpDeskApplicationTestBase
    {
        private readonly ITicketAppService _ticketAppService;

        public TicketAppService_Tests()
        {
            _ticketAppService = ServiceProvider.GetRequiredService<ITicketAppService>();
        }

        [Fact]
        public void GetAll_Test()
        {
            //Act

            var result = _ticketAppService.GetAll(new GetAllTicketsInput());

            //Assert

            result.Items.Count.ShouldBe(1);
        }

        [Fact]
        public void GetAll_Filtered_Test()
        {
            //Act

            var result = _ticketAppService.GetAll(new GetAllTicketsInput { Filter = "non-existing-text" });

            //Assert

            result.Items.Count.ShouldBe(0);
        }
    }
}
