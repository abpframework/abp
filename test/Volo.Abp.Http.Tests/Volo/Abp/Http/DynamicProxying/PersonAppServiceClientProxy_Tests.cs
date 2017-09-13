using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TestApp.Application;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying
{
    public class PersonAppServiceClientProxy_Tests : AbpHttpTestBase
    {
        private readonly IPeopleAppService _peopleAppService;

        public PersonAppServiceClientProxy_Tests()
        {
            //TODO: Should actually test the proxy!
            _peopleAppService = ServiceProvider.GetRequiredService<IPeopleAppService>();
        }

        [Fact]
        public async Task Test_GetList()
        {
            var people = await _peopleAppService.GetList(new PagedAndSortedResultRequestDto());
            people.TotalCount.ShouldBeGreaterThan(0);
            people.Items.Count.ShouldBe(people.TotalCount);
        }
    }
}
