using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.Auditing;

public class Auditing_AsyncEnumerable_ScopeFailure_Tests : AbpAuditingTestBase
{
    private readonly IAuditingManager _auditingManager;
    private readonly FailingAuditingHelperState _state;
    private readonly Auditing_AsyncEnumerable_Tests.MyStreamingAuditedObject _target;

    public Auditing_AsyncEnumerable_ScopeFailure_Tests()
    {
        _auditingManager = GetRequiredService<IAuditingManager>();
        _state = GetRequiredService<FailingAuditingHelperState>();
        _target = GetRequiredService<Auditing_AsyncEnumerable_Tests.MyStreamingAuditedObject>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddSingleton<FailingAuditingHelperState>();
        services.Replace(ServiceDescriptor.Transient<IAuditingHelper, FailingAuditingHelper>());
        services.Replace(ServiceDescriptor.Singleton(Substitute.For<IAuditingStore>()));
    }

    [Fact]
    public async Task Should_Dispose_The_Scope_Of_A_New_Audit_Log_If_Beginning_The_Enumeration_Audit_Fails()
    {
        var sequence = _target.StreamItemsAsync(1);
        _state.FailToCreateAction = true;

        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(sequence));

        _state.IsFailedHelperDisposed.ShouldBeTrue();
        _auditingManager.Current.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Dispose_The_Scope_If_Beginning_The_Enumeration_Audit_In_The_Current_Audit_Log_Fails()
    {
        using (_auditingManager.BeginScope())
        {
            var auditLog = _auditingManager.Current!.Log;
            var sequence = _target.StreamItemsAsync(1);
            _state.FailToCreateAction = true;

            await Should.ThrowAsync<InvalidOperationException>(ToListAsync(sequence));

            _state.IsFailedHelperDisposed.ShouldBeTrue();
            _auditingManager.Current!.Log.ShouldBeSameAs(auditLog);
        }
    }

    private static async Task<List<int>> ToListAsync(IAsyncEnumerable<int> source)
    {
        var list = new List<int>();
        await foreach (var item in source)
        {
            list.Add(item);
        }

        return list;
    }

    public class FailingAuditingHelperState
    {
        public bool FailToCreateAction { get; set; }

        public bool IsFailedHelperDisposed { get; set; }
    }

    [DisableConventionalRegistration]
    public class FailingAuditingHelper : AuditingHelper, IDisposable
    {
        private readonly FailingAuditingHelperState _state;
        private bool _hasFailed;

        public FailingAuditingHelper(
            FailingAuditingHelperState state,
            IAuditSerializer auditSerializer,
            IOptions<AbpAuditingOptions> options,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            ICurrentClient currentClient,
            IClock clock,
            IAuditingStore auditingStore,
            ILogger<AuditingHelper> logger,
            IServiceProvider serviceProvider,
            ICorrelationIdProvider correlationIdProvider,
            IAmbientScopeProvider<AuditingDisabledState> auditingDisabledState)
            : base(auditSerializer, options, currentUser, currentTenant, currentClient, clock, auditingStore, logger, serviceProvider, correlationIdProvider, auditingDisabledState)
        {
            _state = state;
        }

        public override AuditLogActionInfo CreateAuditLogAction(AuditLogInfo auditLog, Type type, MethodInfo method, object[] arguments)
        {
            if (_state.FailToCreateAction)
            {
                _hasFailed = true;
                throw new InvalidOperationException("Failed to create the audit log action");
            }

            return base.CreateAuditLogAction(auditLog, type, method, arguments);
        }

        public void Dispose()
        {
            if (_hasFailed)
            {
                _state.IsFailedHelperDisposed = true;
            }
        }
    }
}
