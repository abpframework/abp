using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.AsyncEnumerable;

public class UnitOfWorkInterceptor_BeginScope_Tests : EntityFrameworkCoreTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddSingleton<FailingTransactionBehaviourState>();
        services.Replace(ServiceDescriptor.Scoped<IUnitOfWorkTransactionBehaviourProvider, FailingTransactionBehaviourProvider>());
    }

    [Fact]
    public async Task Should_Dispose_The_Service_Scope_If_Beginning_The_Unit_Of_Work_Fails()
    {
        var state = GetRequiredService<FailingTransactionBehaviourState>();
        var repository = GetRequiredService<IRepository<Person, Guid>>();
        state.Fail = true;

        await Should.ThrowAsync<InvalidOperationException>(() => repository.GetListAsync());

        state.IsFailedProviderDisposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Dispose_The_Service_Scope_If_Beginning_The_Unit_Of_Work_Of_An_Enumeration_Fails()
    {
        var state = GetRequiredService<FailingTransactionBehaviourState>();
        var sequence = GetRequiredService<IStreamingPeopleAppService>().GetNamesAsync();
        state.Fail = true;

        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in sequence)
            {
            }
        });

        state.IsFailedProviderDisposed.ShouldBeTrue();
    }

    public class FailingTransactionBehaviourState
    {
        public bool Fail { get; set; }

        public bool IsFailedProviderDisposed { get; set; }
    }

    [DisableConventionalRegistration]
    public class FailingTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider, IDisposable
    {
        private readonly FailingTransactionBehaviourState _state;
        private bool _hasFailed;

        public FailingTransactionBehaviourProvider(FailingTransactionBehaviourState state)
        {
            _state = state;
        }

        public bool? IsTransactional
        {
            get
            {
                if (_state.Fail)
                {
                    _hasFailed = true;
                    throw new InvalidOperationException("Failed to get the transaction behaviour");
                }

                return null;
            }
        }

        public void Dispose()
        {
            if (_hasFailed)
            {
                _state.IsFailedProviderDisposed = true;
            }
        }
    }
}
