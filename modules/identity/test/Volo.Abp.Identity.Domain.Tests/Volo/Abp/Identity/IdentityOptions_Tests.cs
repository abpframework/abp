using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityOptions_Tests : AbpIdentityDomainTestBase
    {
        private ISettingProvider _settingProvider;

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _settingProvider = Substitute.For<ISettingProvider>();
            _settingProvider.GetOrNullAsync(Arg.Any<string>()).Returns((string) null);
            services.Replace(ServiceDescriptor.Singleton(_settingProvider));
        }

        [Fact]
        public void Should_Resolve_AbpIdentityOptionsManager()
        {
            GetRequiredService<IOptions<IdentityOptions>>().ShouldBeOfType(typeof(AbpIdentityOptionsManager));
        }

        [Fact]
        public async Task Should_Get_Options_From_Custom_Settings_If_Available()
        {
            using (var scope1 = ServiceProvider.CreateScope())
            {
                var options = scope1.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>();

                //Can not get the values from the SettingProvider without options.SetAsync();

                options.Value.Password.RequiredLength.ShouldBe(6); //Default value
                options.Value.Password.RequiredUniqueChars.ShouldBe(1); //Default value
            }

            using (var scope2 = ServiceProvider.CreateScope())
            {
                var options = scope2.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>();
                var optionsValue = options.Value;

                await options.SetAsync();

                //Still the default values because SettingProvider has not been configured yet

                optionsValue.Password.RequiredLength.ShouldBe(6); //Default value
                optionsValue.Password.RequiredUniqueChars.ShouldBe(1); //Default value
            }

            _settingProvider
                .GetOrNullAsync(IdentitySettingNames.Password.RequiredLength)
                .Returns(Task.FromResult("42"));

            using (var scope2 = ServiceProvider.CreateScope())
            {
                var options = scope2.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>();
                var optionsValue = options.Value;

                await options.SetAsync();

                //Get the value from SettingProvider

                optionsValue.Password.RequiredLength.ShouldBe(42); //Setting value
                optionsValue.Password.RequiredUniqueChars.ShouldBe(1); //Default value
            }
        }
    }
}
