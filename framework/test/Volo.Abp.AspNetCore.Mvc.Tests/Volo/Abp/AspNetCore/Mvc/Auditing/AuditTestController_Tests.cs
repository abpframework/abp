﻿using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Auditing;

public class AuditTestController_Tests : AspNetCoreMvcTestBase
{
    private readonly AbpAuditingOptions _options;
    private IAuditingStore _auditingStore;

    public AuditTestController_Tests()
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
        await GetResponseAsync("/api/audit-test/");
        await _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x =>
            x.Actions.Any(a => a.ServiceName == typeof(AuditTestController).FullName) &&
            x.Actions.Any(a => a.MethodName == nameof(AuditTestController.Get))));
    }

    [Fact]
    public async Task Should_Trigger_Middleware_And_AuditLog_Success_For_GetRequests()
    {
        _options.IsEnabledForGetRequests = true;
        _options.AlwaysLogOnException = false;
        await GetResponseAsync("api/audit-test/audit-success");
        await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }


    [Fact]
    public async Task Should_Trigger_Middleware_And_AuditLog_Success_For_Specified_Requests()
    {
        _options.AlwaysLogOnException = false;
        _options.AlwaysLogSelectors.Add(info => Task.FromResult(info.Url.Contains("api/audit-test/audit-success")));
        await GetResponseAsync("api/audit-test/audit-success");
        await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public async Task Should_Trigger_Middleware_And_AuditLog_Exception_Always()
    {
        _options.IsEnabled = true;
        _options.AlwaysLogOnException = true;

        try
        {
            await GetResponseAsync("api/audit-test/audit-fail", System.Net.HttpStatusCode.Forbidden);
        }
        catch { }

        await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public async Task Should_Trigger_Middleware_And_AuditLog_Exception_When_Returns_Object()
    {
        _options.IsEnabled = true;
        _options.AlwaysLogOnException = true;

        await GetResponseAsync("api/audit-test/audit-fail-object", System.Net.HttpStatusCode.Forbidden);

        await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
    }

    [Fact]
    public async Task Should_Trigger_Middleware_And_AuditLog_Exception_When_Activate_Controller_Failed()
    {
        _options.IsEnabledForGetRequests = true;
        _options.AlwaysLogOnException = true;

        try
        {
            await GetResponseAsync("api/audit-test/audit-activate-failed", System.Net.HttpStatusCode.InternalServerError);
        }
        catch { }

        await _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x => x.Exceptions.Any()));
    }
}
