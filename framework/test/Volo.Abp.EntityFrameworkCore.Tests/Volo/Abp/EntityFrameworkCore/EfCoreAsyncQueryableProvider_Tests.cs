using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore
{
    public class EfCoreAsyncQueryableProvider_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly EfCoreAsyncQueryableProvider _efCoreAsyncQueryableProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EfCoreAsyncQueryableProvider_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            _efCoreAsyncQueryableProvider = GetRequiredService<EfCoreAsyncQueryableProvider>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public void Should_Accept_EfCore_Related_Queries()
        {
            var query = _personRepository.Where(p => p.Age > 0);
            
            _efCoreAsyncQueryableProvider.CanExecute(query).ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Accept_Other_Providers()
        {
            var query = new[] {1, 2, 3}.AsQueryable().Where(x => x > 0);
            
            _efCoreAsyncQueryableProvider.CanExecute(query).ShouldBeFalse();
        }
        
        [Fact]
        public async Task Should_Execute_Queries()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var query = _personRepository.Where(p => p.Age > 0);
            
                (await _efCoreAsyncQueryableProvider.CountAsync(query) > 0).ShouldBeTrue();
                (await _efCoreAsyncQueryableProvider.FirstOrDefaultAsync(query)).ShouldNotBeNull();
                (await _efCoreAsyncQueryableProvider.ToListAsync(query)).Count.ShouldBeGreaterThan(0);

                await uow.CompleteAsync();
            }
        }
    }
}