using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class AbpDateTimeValueConverter_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IBasicRepository<Person, Guid> _personRepository;

        protected AbpDateTimeValueConverter_Tests()
        {
            _personRepository = GetRequiredService<IBasicRepository<Person, Guid>>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<AbpClockOptions>(options => options.Kind = DateTimeKind.Utc);
        }

        [Fact]
        public async Task DateTime_Kind_Should_Be_Normalized_To_UTC_Test()
        {
            var personId = Guid.Parse("4125582e-d100-4c27-aa84-e4de85830dca");
            await _personRepository.InsertAsync(new Person(personId, "bob lee", 18)
            {
                Birthday = DateTime.Parse("2020-01-01 00:00:00"),
                LastActive = DateTime.Parse("2020-01-01 00:00:00"),
            }, true);

            var person = await _personRepository.GetAsync(personId);

            person.ShouldNotBeNull();
            person.CreationTime.Kind.ShouldBe(DateTimeKind.Utc);

            person.Birthday.ShouldNotBeNull();
            person.Birthday.Value.Kind.ShouldBe(DateTimeKind.Utc);
            person.Birthday.Value.ToString("yyy-MM-dd HH:mm:ss").ShouldBe("2020-01-01 00:00:00");

            //LastActive DisableDateTimeNormalization
            person.LastActive.ShouldNotBeNull();
            person.LastActive.Value.Kind.ShouldBe(DateTimeKind.Unspecified);
            person.LastActive.Value.ToString("yyy-MM-dd HH:mm:ss").ShouldBe("2020-01-01 00:00:00");
        }
    }
}