using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters
{
    public abstract class EFCore_DateTimeKindTests : DateTimeKind_Tests<AbpEntityFrameworkCoreTestModule>
    {
        [Fact]
        public async Task DateTime_Kind_Should_Be_Normalized_In_View_Query_Test()
        {
            var personName = "bob lee";
            await PersonRepository.InsertAsync(new Person(Guid.NewGuid(), personName, 18)
            {
                Birthday = DateTime.Parse("2020-01-01 00:00:00"),
                LastActive = DateTime.Parse("2020-01-01 00:00:00"),
            }, true);

            var person = await PersonRepository.GetViewAsync(personName);

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

    public class DateTimeKindTests : EFCore_DateTimeKindTests
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            Kind = DateTimeKind.Unspecified;
            services.Configure<AbpClockOptions>(x => x.Kind = Kind);
        }
    }

    public class DateTimeKindTests_Local : EFCore_DateTimeKindTests
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            Kind = DateTimeKind.Local;
            services.Configure<AbpClockOptions>(x => x.Kind = Kind);
        }
    }

    public class DateTimeKindTests_Utc : EFCore_DateTimeKindTests
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            Kind = DateTimeKind.Utc;
            services.Configure<AbpClockOptions>(x => x.Kind = Kind);
        }
    }
}
