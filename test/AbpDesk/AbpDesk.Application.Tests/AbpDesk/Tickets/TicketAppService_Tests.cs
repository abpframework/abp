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

            var result = _ticketAppService.GetAll();

            //Assert

            result.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
