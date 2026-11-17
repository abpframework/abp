using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Auditing;
using Volo.Abp.DynamicProxy;
using Volo.Abp.TestApp.Application;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Auditing;

public class StreamingAppService_Auditing_Tests : AspNetCoreMvcTestBase
{
    private readonly AbpAuditingOptions _options;
    private IAuditingStore _auditingStore;
    private readonly List<AuditLogInfo> _savedAuditLogs = new();

    public StreamingAppService_Auditing_Tests()
    {
        _options = ServiceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
        _auditingStore = ServiceProvider.GetRequiredService<IAuditingStore>();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        _auditingStore = Substitute.For<IAuditingStore>();
        _auditingStore.SaveAsync(Arg.Do<AuditLogInfo>(_savedAuditLogs.Add));
        services.Replace(ServiceDescriptor.Singleton(_auditingStore));
        base.ConfigureServices(services);
    }

    [Fact]
    public async Task Should_Audit_The_Call_And_The_Enumeration_Of_A_Streaming_Action()
    {
        _options.IsEnabledForGetRequests = true;
        ProxyHelper.IsProxy(ServiceProvider.GetRequiredService<StreamingAppService>()).ShouldBeTrue();

        var result = await GetResponseAsObjectAsync<List<int>>("/api/app/streaming/items");

        result.ShouldBe(new[] { 1, 2 });
        GetRequiredService<AuditingConcernProbe>().Results.ShouldBe(new[] { true });
        _savedAuditLogs.Count.ShouldBe(1);
        var actions = _savedAuditLogs[0].Actions.Where(a => a.MethodName == nameof(StreamingAppService.GetItemsAsync)).ToList();
        actions.Count.ShouldBe(2);
        actions.Count(IsEnumeration).ShouldBe(1);
    }

    [Fact]
    public async Task Should_Record_The_Duration_Of_The_Enumeration_Of_A_Streaming_Action()
    {
        _options.IsEnabledForGetRequests = true;

        var result = await GetResponseAsObjectAsync<List<int>>("/api/app/streaming/slow-items");

        result.ShouldBe(new[] { 1 });
        var enumeration = _savedAuditLogs.Single().Actions.Single(a => a.MethodName == nameof(StreamingAppService.GetSlowItemsAsync) && IsEnumeration(a));
        enumeration.ExecutionDuration.ShouldBeGreaterThanOrEqualTo(40);
    }

    [Fact]
    public async Task Should_Not_Audit_The_Enumeration_When_The_Streaming_Action_Is_Not_Audited()
    {
        _options.IsEnabledForGetRequests = false;

        var result = await GetResponseAsObjectAsync<List<int>>("/api/app/streaming/items");

        result.ShouldBe(new[] { 1, 2 });
        _savedAuditLogs.SelectMany(x => x.Actions).ShouldNotContain(a => a.MethodName == nameof(StreamingAppService.GetItemsAsync));
    }

    [Theory]
    [InlineData("/api/app/streaming/items-with-creation-error")]
    [InlineData("/api/streaming-json-test/creation-error")]
    public async Task Should_Handle_Exception_Thrown_While_Creating_The_Sequence_As_An_Action_Exception(string url)
    {
        _options.IsEnabledForGetRequests = true;

        // AbpExceptionFilter only handles exceptions of actions returning IActionResult for AJAX requests.
        using (var response = await GetResponseAsync(url, HttpStatusCode.Forbidden, xmlHttpRequest: true))
        {
            (await response.Content.ReadAsStringAsync()).ShouldContain("Failed to create the sequence");
            _savedAuditLogs.Single().Exceptions.ShouldContain(e => e.Message == "Failed to create the sequence");
        }
    }

    [Fact]
    public async Task Should_Wait_For_Interceptors_Of_A_Sequence_Returned_After_A_Handled_Exception()
    {
        _options.IsEnabledForGetRequests = true;

        var result = await GetResponseAsObjectAsync<List<int>>("/api/streaming-json-test/handled-exception");

        result.ShouldBe(new[] { 1, 2 });
        var controllerAction = _savedAuditLogs.Single().Actions.Single(a => a.ServiceName == typeof(StreamingJsonTestController).FullName);
        controllerAction.ExecutionDuration.ShouldBeGreaterThanOrEqualTo(40);
    }

    [Fact]
    public async Task Should_Wait_For_Interceptors_Of_A_Sequence_Returned_In_A_JsonResult()
    {
        _options.IsEnabledForGetRequests = true;

        var result = await GetResponseAsObjectAsync<List<int>>("/api/streaming-json-test");

        result.ShouldBe(new[] { 1, 2 });

        // The interceptors of the sequence (delayed by the probe) run within the action, like for an ObjectResult.
        var controllerAction = _savedAuditLogs.Single().Actions.Single(a => a.ServiceName == typeof(StreamingJsonTestController).FullName);
        controllerAction.ExecutionDuration.ShouldBeGreaterThanOrEqualTo(40);
    }

    private static bool IsEnumeration(AuditLogActionInfo action)
    {
        return action.ExtraProperties.ContainsKey(AuditingInterceptor.AsyncEnumerationPropertyName);
    }
}
