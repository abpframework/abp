using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Events;
using Shouldly;
using Volo.Abp.AspNetCore.App;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.AspNetCore.Serilog
{
    public class Serilog_Enrichers_Tests : AbpSerilogTestBase
    {
        private const string ExecutedEndpointLogEventText = "Executed endpoint '{EndpointName}'";

        private readonly Guid _testTenantId = Guid.NewGuid();
        private readonly string _testTenantName = "acme";

        private readonly AbpAspNetCoreMultiTenancyOptions _tenancyOptions;
        private readonly AbpAspNetCoreSerilogEnrichersOptions _serilogEnrichersOptions;
        private readonly ILogger<Serilog_Enrichers_Tests> _logger;

        public Serilog_Enrichers_Tests()
        {
            _tenancyOptions = ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value;
            _serilogEnrichersOptions =
                ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreSerilogEnrichersOptions>>().Value;
            _logger = ServiceProvider.GetRequiredService<ILogger<Serilog_Enrichers_Tests>>();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder().ConfigureServices(services =>
            {
                services.Configure<AbpDefaultTenantStoreOptions>(options =>
                {
                    options.Tenants = new[]
                    {
                        new TenantConfiguration(_testTenantId, _testTenantName)
                    };
                });
            });
        }

        [Fact]
        public async Task TenantId_Not_Set_Test()
        {
            var url = GetUrl<SerilogTestController>(nameof(SerilogTestController.Index));
            var result = await GetResponseAsStringAsync(url).ConfigureAwait(false);

            var executedLogEvent = GetLogEvent(ExecutedEndpointLogEventText);

            executedLogEvent.ShouldNotBeNull();
            executedLogEvent.Properties.ContainsKey(_serilogEnrichersOptions.TenantIdEnricherPropertyName)
                .ShouldBe(false);
        }

        [Fact]
        public async Task TenantId_Set_Test()
        {
            var url =
                GetUrl<SerilogTestController>(nameof(SerilogTestController.Index)) +
                $"?{_tenancyOptions.TenantKey}={_testTenantName}";
            var result = await GetResponseAsStringAsync(url).ConfigureAwait(false);

            var executedLogEvent = GetLogEvent(ExecutedEndpointLogEventText);

            executedLogEvent.ShouldNotBeNull();
            executedLogEvent.Properties.ContainsKey(_serilogEnrichersOptions.TenantIdEnricherPropertyName)
                .ShouldBe(true);
            ((ScalarValue) executedLogEvent.Properties[_serilogEnrichersOptions.TenantIdEnricherPropertyName]).Value
                .ShouldBe(_testTenantId);
        }

        [Fact]
        public async Task CorrelationId_Enrichers_Test()
        {
            var url = GetUrl<SerilogTestController>(nameof(SerilogTestController.CorrelationId));
            var result = await GetResponseAsStringAsync(url).ConfigureAwait(false);

            var executedLogEvent = GetLogEvent(ExecutedEndpointLogEventText);

            executedLogEvent.ShouldNotBeNull();

            executedLogEvent.Properties.ContainsKey(_serilogEnrichersOptions.CorrelationIdPropertyName)
                .ShouldNotBeNull();

            ((ScalarValue) executedLogEvent.Properties[_serilogEnrichersOptions.CorrelationIdPropertyName]).Value
                .ShouldBe(result);
        }
    }
}