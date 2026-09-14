using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xunit;

namespace Volo.Abp.Auditing;

public class AuditingInterceptor_HttpMethod_Tests : AbpAuditingTestBase
{
    protected IAuditingStore AuditingStore;

    private string? _httpMethod;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        AuditingStore = Substitute.For<IAuditingStore>();
        services.Replace(ServiceDescriptor.Singleton(AuditingStore));

        services.Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabledForGetRequests = false;
            options.Contributors.Add(new TestHttpMethodAuditContributor(() => _httpMethod));
        });
    }

    [Fact]
    public async Task Should_Not_Write_AuditLog_For_Query_Http_Method_Without_Explicit_Scope()
    {
        _httpMethod = "QUERY";

        var auditedObject = GetRequiredService<Auditing_Tests.MyAuditedObject1>();
        await auditedObject.DoItAsync(new Auditing_Tests.InputObject { Value1 = "x", Value2 = 1 });

        await AuditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public async Task Should_Write_AuditLog_For_Post_Http_Method_Without_Explicit_Scope()
    {
        _httpMethod = "POST";

        var auditedObject = GetRequiredService<Auditing_Tests.MyAuditedObject1>();
        await auditedObject.DoItAsync(new Auditing_Tests.InputObject { Value1 = "x", Value2 = 1 });

        await AuditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    public class TestHttpMethodAuditContributor : AuditLogContributor
    {
        private readonly Func<string?> _httpMethodFactory;

        public TestHttpMethodAuditContributor(Func<string?> httpMethodFactory)
        {
            _httpMethodFactory = httpMethodFactory;
        }

        public override void PreContribute(AuditLogContributionContext context)
        {
            context.AuditInfo.HttpMethod = _httpMethodFactory();
        }
    }
}
