using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AspNetCore.SignalR.SampleHubs;
using Xunit;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class AbpSignalROptions_Tests : AbpAspNetCoreSignalRTestBase
    {
        private readonly AbpSignalROptions _options;

        public AbpSignalROptions_Tests()
        {
            _options = GetRequiredService<IOptions<AbpSignalROptions>>().Value;
        }

        [Fact(Skip = "Can not run this test since AspNet Core environment has not been properly set!")]
        public void Should_Auto_Add_Maps()
        {
            _options.Hubs.ShouldContain(h => h.HubType == typeof(RegularHub));
            _options.Hubs.ShouldContain(h => h.HubType == typeof(RegularAbpHub));
            _options.Hubs.ShouldNotContain(h => h.HubType == typeof(DisableConventionalRegistrationHub));
            _options.Hubs.ShouldNotContain(h => h.HubType == typeof(DisableAutoHubMapHub));
        }
    }
}
