using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading.Tasks;
using NSubstitute;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Application.Dto;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Application
{
    public class PersonAppService_Tests : TestAppTestBase
    {
        private readonly IPeopleAppService _peopleAppService;
        private ICurrentTenant _fakeCurrentTenant;

        public PersonAppService_Tests()
        {
            _peopleAppService = ServiceProvider.GetRequiredService<IPeopleAppService>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _fakeCurrentTenant = Substitute.For<ICurrentTenant>();
            services.AddSingleton(_fakeCurrentTenant);
        }

        [Fact]
        public async Task GetList()
        {
            var people = await _peopleAppService.GetListAsync(new PagedAndSortedResultRequestDto())
                ;
            people.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Create()
        {
            var uniquePersonName = Guid.NewGuid().ToString();
            var personDto = await _peopleAppService.CreateAsync(new PersonDto {Name = uniquePersonName});

            var repository = ServiceProvider.GetService<IRepository<Person, Guid>>();
            var person = await repository.FindAsync(personDto.Id);

            person.ShouldNotBeNull();
            person.TenantId.ShouldBeNull();
        }

        [Fact]
        public async Task Create_SetsTenantId()
        {
            _fakeCurrentTenant.Id.Returns(TestDataBuilder.TenantId1);

            var uniquePersonName = Guid.NewGuid().ToString();
            var personDto = await _peopleAppService.CreateAsync(new PersonDto {Name = uniquePersonName});

            var repository = ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
            var person = await repository.FindAsync(personDto.Id);

            person.ShouldNotBeNull();
            person.TenantId.ShouldNotBeNull();
            person.TenantId.ShouldNotBe(Guid.Empty);
            person.TenantId.ShouldBe(TestDataBuilder.TenantId1);
        }
    }
}