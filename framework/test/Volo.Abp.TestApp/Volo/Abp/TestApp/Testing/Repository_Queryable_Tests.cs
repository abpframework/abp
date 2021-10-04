using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class Repository_Queryable_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<Person, Guid> PersonRepository;

        protected Repository_Queryable_Tests()
        {
            PersonRepository = ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
        }

        [Fact]
        public async Task Any()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                (await PersonRepository.AnyAsync()).ShouldBeTrue();
                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task Single()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var person = await PersonRepository.SingleAsync(p => p.Id == TestDataBuilder.UserDouglasId);
                person.Name.ShouldBe("Douglas");
                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task WithDetails()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var person = (await PersonRepository.WithDetailsAsync()).Single(p => p.Id == TestDataBuilder.UserDouglasId);
                person.Name.ShouldBe("Douglas");
                person.Phones.Count.ShouldBe(2);
            });
        }

        [Fact]
        public async Task WithDetails_Explicit()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var person = (await PersonRepository.WithDetailsAsync(p => p.Phones)).Single(p => p.Id == TestDataBuilder.UserDouglasId);
                person.Name.ShouldBe("Douglas");
                person.Phones.Count.ShouldBe(2);
            });
        }
    }
}
