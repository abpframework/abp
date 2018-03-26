using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.MongoDB
{
    public class MongoDb_Repository_Tests : MongoDbTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;

        public MongoDb_Repository_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
        }

        [Fact]
        public async Task GetAsync()
        {
            (await _personRepository.GetAsync(TestDataBuilder.UserDouglasId)).ShouldNotBeNull();
        }
    }
}
