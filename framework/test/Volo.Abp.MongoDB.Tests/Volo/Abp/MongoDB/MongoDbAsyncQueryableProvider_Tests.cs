using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class MongoDbAsyncQueryableProvider_Tests : MongoDbTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly MongoDbAsyncQueryableProvider _mongoDbAsyncQueryableProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MongoDbAsyncQueryableProvider_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            _mongoDbAsyncQueryableProvider = GetRequiredService<MongoDbAsyncQueryableProvider>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Accept_MongoDb_Related_Queries()
        {
            var query = (await _personRepository.GetQueryableAsync()).Where(p => p.Age > 0);

            _mongoDbAsyncQueryableProvider.CanExecute(query).ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Accept_Other_Providers()
        {
            var query = new[] {1, 2, 3}.AsQueryable().Where(x => x > 0);

            _mongoDbAsyncQueryableProvider.CanExecute(query).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Execute_Queries()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var query = (await _personRepository.GetQueryableAsync()).Where(p => p.Age > 0).OrderBy(p => p.Name);

                (await _mongoDbAsyncQueryableProvider.CountAsync(query) > 0).ShouldBeTrue();
                (await _mongoDbAsyncQueryableProvider.FirstOrDefaultAsync(query)).ShouldNotBeNull();
                (await _mongoDbAsyncQueryableProvider.ToListAsync(query)).Count.ShouldBeGreaterThan(0);

                await uow.CompleteAsync();
            }
        }
    }
}
