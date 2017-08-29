using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace Volo.Abp.TestApp.Application
{
    public class PersonAppService_Tests : TestAppTestBase
    {
        private readonly IPersonAppService _personAppService;

        public PersonAppService_Tests()
        {
            _personAppService =  ServiceProvider.GetRequiredService<IPersonAppService>();
        }

        [Fact]
        public async Task GetAll()
        {
            var people = await _personAppService.GetAll(new PagedAndSortedResultRequestDto());
            people.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
