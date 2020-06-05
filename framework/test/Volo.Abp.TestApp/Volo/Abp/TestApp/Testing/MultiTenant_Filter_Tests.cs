using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class MultiTenant_Filter_Tests<TStartupModule> : TestAppTestBase<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        private ICurrentTenant _fakeCurrentTenant;
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly IDataFilter<IMultiTenant> _multiTenantFilter;

        protected MultiTenant_Filter_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            _multiTenantFilter = GetRequiredService<IDataFilter<IMultiTenant>>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _fakeCurrentTenant = Substitute.For<ICurrentTenant>();
            services.AddSingleton(_fakeCurrentTenant);
        }

        [Fact]
        public async Task Should_Get_Person_For_Current_Tenant()
        {
            await WithUnitOfWorkAsync(() =>
            {
                //TenantId = null

                _fakeCurrentTenant.Id.Returns((Guid?)null);

                var people = _personRepository.ToList();
                people.Count.ShouldBe(1);
                people.Any(p => p.Name == "Douglas").ShouldBeTrue();

                //TenantId = TestDataBuilder.TenantId1

                _fakeCurrentTenant.Id.Returns(TestDataBuilder.TenantId1);

                people = _personRepository.ToList();
                people.Count.ShouldBe(2);
                people.Any(p => p.Name == TestDataBuilder.TenantId1 + "-Person1").ShouldBeTrue();
                people.Any(p => p.Name == TestDataBuilder.TenantId1 + "-Person2").ShouldBeTrue();

                //TenantId = TestDataBuilder.TenantId2

                _fakeCurrentTenant.Id.Returns(TestDataBuilder.TenantId2);

                people = _personRepository.ToList();
                people.Count.ShouldBe(0);

                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task Should_Get_All_People_When_MultiTenant_Filter_Is_Disabled()
        {
            await WithUnitOfWorkAsync(() =>
            {
                List<Person> people;

                using (_multiTenantFilter.Disable())
                {
                    //Filter disabled manually
                    people = _personRepository.ToList();
                    people.Count.ShouldBe(3);
                }

                //Filter re-enabled automatically
                people = _personRepository.ToList();
                people.Count.ShouldBe(1);

                return Task.CompletedTask;
            });
        }
    }
}
