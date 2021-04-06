using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using Volo.Abp.Auditing;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Auditing
{
    public class AuditTestPage_Tests : AspNetCoreMvcTestBase
    {
        private readonly AbpAuditingOptions _options;
        private IAuditingStore _auditingStore;

        public AuditTestPage_Tests()
        {
            _options = ServiceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
            _auditingStore = ServiceProvider.GetRequiredService<IAuditingStore>();
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            _auditingStore = Substitute.For<IAuditingStore>();
            services.Replace(ServiceDescriptor.Singleton(_auditingStore));
            base.ConfigureServices(context, services);
        }

        [Fact]
        public async Task Should_Get_Correct_ServiceName_And_MethodName()
        {
            _options.IsEnabledForGetRequests = true;
            _options.AlwaysLogOnException = false;
            await GetResponseAsync("/Auditing/AuditTestPage");
            await _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x =>
                x.Actions.Any(a => a.ServiceName == typeof(AuditTestPage).FullName) &&
                x.Actions.Any(a => a.MethodName == nameof(AuditTestPage.OnGet))));
        }

        [Fact]
        public async Task Should_Trigger_Middleware_And_AuditLog_Success_For_GetRequests()
        {
            _options.IsEnabledForGetRequests = true;
            _options.AlwaysLogOnException = false;
            await GetResponseAsync("/Auditing/AuditTestPage?handler=AuditSuccessForGetRequests");
            await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
        }

        [Fact]
        public async Task Should_Trigger_Middleware_And_AuditLog_Exception_Always()
        {
            _options.IsEnabled = true;
            _options.AlwaysLogOnException = true;

            try
            {
                await GetResponseAsync("/Auditing/AuditTestPage?handler=AuditFailForGetRequests", System.Net.HttpStatusCode.Forbidden);
            }
            catch { }

            await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
        }

        [Fact]
        public async Task Should_Trigger_Middleware_And_AuditLog_Exception_When_Returns_Object()
        {
            _options.IsEnabled = true;
            _options.AlwaysLogOnException = true;

            await GetResponseAsync("/Auditing/AuditTestPage?handler=AuditFailForGetRequestsReturningObject", System.Net.HttpStatusCode.Forbidden);

            await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
        }
    }
}
