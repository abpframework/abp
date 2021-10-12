using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class DateTimeKind_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IPersonRepository PersonRepository { get; }
        protected DateTimeKind Kind { get; set; }

        protected DateTimeKind_Tests()
        {
            PersonRepository = GetRequiredService<IPersonRepository>();
        }

        [Fact]
        public async Task DateTime_Kind_Should_Be_Normalized_Test()
        {
            var personId = Guid.NewGuid();
            await PersonRepository.InsertAsync(new Person(personId, "bob lee", 18)
            {
                Birthday = DateTime.Parse("2020-01-01 00:00:00"),
                LastActive = DateTime.Parse("2020-01-01 00:00:00"),
            }, true);

            var person = await PersonRepository.GetAsync(personId);

            person.ShouldNotBeNull();
            person.CreationTime.Kind.ShouldBe(Kind);

            person.Birthday.ShouldNotBeNull();
            person.Birthday.Value.Kind.ShouldBe(Kind);
            person.Birthday.Value.ToString("yyy-MM-dd HH:mm:ss").ShouldBe("2020-01-01 00:00:00");

            //LastActive DisableDateTimeNormalization
            person.LastActive.ShouldNotBeNull();
            person.LastActive.Value.Kind.ShouldBe(DateTimeKind.Unspecified);
            person.LastActive.Value.ToString("yyy-MM-dd HH:mm:ss").ShouldBe("2020-01-01 00:00:00");
        }
    }
}
