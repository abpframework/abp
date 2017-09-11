using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace Volo.Abp.TestApp.Application
{
    public class PersonAppService_Tests : TestAppTestBase
    {
        private readonly IPeopleAppService _peopleAppService;

        public PersonAppService_Tests()
        {
            _peopleAppService =  ServiceProvider.GetRequiredService<IPeopleAppService>();
        }

        [Fact]
        public async Task GetAll()
        {
            var people = await _peopleAppService.GetList(new PagedAndSortedResultRequestDto());
            people.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
