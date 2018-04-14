using System;
using System.Linq;
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
        public void Any()
        {
            WithUnitOfWork(() =>
            {
                PersonRepository.Any().ShouldBeTrue();
            });
        }

        [Fact]
        public void Single()
        {
            WithUnitOfWork(() =>
            {
                var person = PersonRepository.Single(p => p.Id == TestDataBuilder.UserDouglasId);
                person.Name.ShouldBe("Douglas");
            });
        }

        [Fact]
        public void WithDetails()
        {
            WithUnitOfWork(() =>
            {
                var person = PersonRepository.WithDetails().Single(p => p.Id == TestDataBuilder.UserDouglasId);
                person.Name.ShouldBe("Douglas");
                person.Phones.Count.ShouldBe(2);
            });
        }

        [Fact]
        public void WithDetails_Explicit()
        {
            WithUnitOfWork(() =>
            {
                var person = PersonRepository.WithDetails(p => p.Phones).Single(p => p.Id == TestDataBuilder.UserDouglasId);
                person.Name.ShouldBe("Douglas");
                person.Phones.Count.ShouldBe(2);
            });
        }
    }
}
