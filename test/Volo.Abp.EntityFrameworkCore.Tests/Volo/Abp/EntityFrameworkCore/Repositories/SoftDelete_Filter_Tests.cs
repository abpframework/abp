using System.Linq;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Repositories
{
    public class SoftDelete_Filter_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person> _personRepository;

        public SoftDelete_Filter_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person>>();
        }

        [Fact]
        public void Should_Not_Get_Deleted_Entities_By_Default()
        {
            var people = _personRepository.GetList();
            people.Count.ShouldBe(1);
            people.Any(p => p.Name == "Douglas").ShouldBeTrue();
        }
    }
}
