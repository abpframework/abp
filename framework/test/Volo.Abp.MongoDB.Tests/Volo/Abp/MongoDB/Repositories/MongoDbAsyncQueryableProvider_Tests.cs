using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.MongoDB.Repositories
{
    [Collection(MongoTestCollection.Name)]
    public class MongoDbAsyncQueryableProvider_Tests : TestAppTestBase<AbpMongoDbTestModule>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly MongoDbAsyncQueryableProvider _mongoDbAsyncQueryableProvider;

        public MongoDbAsyncQueryableProvider_Tests()
        {
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            _mongoDbAsyncQueryableProvider = GetRequiredService<MongoDbAsyncQueryableProvider>();
        }

        [Fact]
        public async Task CanExecuteAsync()
        {
            _mongoDbAsyncQueryableProvider.CanExecute(_personRepository).ShouldBeTrue();
            _mongoDbAsyncQueryableProvider.CanExecute(await _personRepository.WithDetailsAsync()).ShouldBeTrue();
        }

        [Fact]
        public async Task FirstOrDefaultAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                (await _mongoDbAsyncQueryableProvider.FirstOrDefaultAsync(_personRepository.Where(p => p.Name == "Douglas"))).ShouldNotBeNull();
                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task AnyAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                (await _mongoDbAsyncQueryableProvider.AnyAsync(_personRepository, p => p.Name == "Douglas")).ShouldBeTrue();
                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task CountAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                (await _mongoDbAsyncQueryableProvider.CountAsync(_personRepository.Where(p => p.Name == "Douglas"))).ShouldBeGreaterThan(0);
                await uow.CompleteAsync();
            }
        }

        [Fact]
        public async Task LongCountAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                (await _mongoDbAsyncQueryableProvider.LongCountAsync(_personRepository)).ShouldBeGreaterThan(0);
                await uow.CompleteAsync();
            }
        }

        //More MongoDbAsyncQueryableProvider's method test.
    }
}
