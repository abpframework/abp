using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Application;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying
{
    public class PersonAppServiceClientProxy_Tests : AbpHttpTestBase
    {
        private readonly IPeopleAppService _peopleAppService;
        private readonly IRepository<Person> _personRepository;

        public PersonAppServiceClientProxy_Tests()
        {
            _peopleAppService = ServiceProvider.GetRequiredService<IPeopleAppService>();
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person>>();
        }

        [Fact]
        public async Task Test_GetList()
        {
            var people = await _peopleAppService.GetList(new PagedAndSortedResultRequestDto());
            people.TotalCount.ShouldBeGreaterThan(0);
            people.Items.Count.ShouldBe(people.TotalCount);
        }

        [Fact]
        public async Task Test_GetById()
        {
            var firstPerson = _personRepository.GetList().First();

            var person = await _peopleAppService.Get(firstPerson.Id);
            person.ShouldNotBeNull();
            person.Id.ShouldBe(firstPerson.Id);
            person.Name.ShouldBe(firstPerson.Name);
        }
    }
}
