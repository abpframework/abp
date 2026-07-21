using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.SimpleStateChecking;

/// <summary>
/// Probe-based tests that verify:
/// 1. Batch path shares a single DI scope but isolates CachedServiceProvider per state.
/// 2. Single-state path uses separate scopes (original behavior).
/// 3. Nested single-state calls from within batch checkers get their own scope.
/// </summary>
public class SimpleStateChecker_ScopeIsolation_Tests : SimpleStateCheckerTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddScoped<ScopeIdProbe>();
        services.AddTransient<TransientIdProbe>();
        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task Batch_Should_Share_Scope_But_Isolate_CachedProvider_Across_States()
    {
        var observations = new ConcurrentDictionary<MyStateEntity, Observation>();
        var checker = new ScopeProbeStateChecker(observations);

        var stateA = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        stateA.AddSimpleStateChecker(checker);

        var stateB = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        stateB.AddSimpleStateChecker(checker);

        var stateC = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        stateC.AddSimpleStateChecker(checker);

        await SimpleStateCheckerManager.IsEnabledAsync(new[] { stateA, stateB, stateC });

        observations.Count.ShouldBe(3);

        // All states in the batch should see the same scoped service (shared DI scope)
        observations[stateA].ScopeId.ShouldBe(observations[stateB].ScopeId);
        observations[stateB].ScopeId.ShouldBe(observations[stateC].ScopeId);

        // Each state should get its own transient instance (isolated cached provider)
        observations[stateA].TransientId.ShouldNotBe(observations[stateB].TransientId);
        observations[stateB].TransientId.ShouldNotBe(observations[stateC].TransientId);
    }

    [Fact]
    public async Task Single_State_Calls_Should_Use_Separate_Scopes()
    {
        var observations = new ConcurrentDictionary<MyStateEntity, Observation>();
        var checker = new ScopeProbeStateChecker(observations);

        var stateA = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        stateA.AddSimpleStateChecker(checker);

        var stateB = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        stateB.AddSimpleStateChecker(checker);

        await SimpleStateCheckerManager.IsEnabledAsync(stateA);
        await SimpleStateCheckerManager.IsEnabledAsync(stateB);

        observations.Count.ShouldBe(2);

        // Single-state calls should each get their own scope
        observations[stateA].ScopeId.ShouldNotBe(observations[stateB].ScopeId);
    }

    [Fact]
    public async Task Nested_Single_State_Call_From_Batch_Checker_Should_Get_Own_Scope()
    {
        var observations = new ConcurrentDictionary<MyStateEntity, Observation>();

        // This state will be evaluated via a nested single-state call during batch evaluation
        var nestedState = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        nestedState.AddSimpleStateChecker(new ScopeProbeStateChecker(observations));

        // This checker triggers a nested IsEnabledAsync(state) during batch evaluation
        var nestedCallChecker = new NestedSingleCallChecker(
            SimpleStateCheckerManager, nestedState, observations);

        var batchState = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        batchState.AddSimpleStateChecker(nestedCallChecker);

        await SimpleStateCheckerManager.IsEnabledAsync(new[] { batchState });

        observations.Count.ShouldBe(2);

        // The nested single-state call should get its own scope, not the batch scope
        observations[batchState].ScopeId.ShouldNotBe(observations[nestedState].ScopeId);
    }

    public record Observation(Guid ScopeId, Guid TransientId);

    public class ScopeIdProbe
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class TransientIdProbe
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class ScopeProbeStateChecker : ISimpleStateChecker<MyStateEntity>
    {
        private readonly ConcurrentDictionary<MyStateEntity, Observation> _observations;

        public ScopeProbeStateChecker(
            ConcurrentDictionary<MyStateEntity, Observation> observations)
        {
            _observations = observations;
        }

        public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<MyStateEntity> context)
        {
            var scopeProbe = context.ServiceProvider.GetRequiredService<ScopeIdProbe>();
            var transientProbe = context.ServiceProvider.GetRequiredService<TransientIdProbe>();
            _observations[context.State] = new Observation(scopeProbe.Id, transientProbe.Id);
            return Task.FromResult(true);
        }
    }

    public class NestedSingleCallChecker : ISimpleStateChecker<MyStateEntity>
    {
        private readonly ISimpleStateCheckerManager<MyStateEntity> _manager;
        private readonly MyStateEntity _nestedState;
        private readonly ConcurrentDictionary<MyStateEntity, Observation> _observations;

        public NestedSingleCallChecker(
            ISimpleStateCheckerManager<MyStateEntity> manager,
            MyStateEntity nestedState,
            ConcurrentDictionary<MyStateEntity, Observation> observations)
        {
            _manager = manager;
            _nestedState = nestedState;
            _observations = observations;
        }

        public async Task<bool> IsEnabledAsync(SimpleStateCheckerContext<MyStateEntity> context)
        {
            var scopeProbe = context.ServiceProvider.GetRequiredService<ScopeIdProbe>();
            var transientProbe = context.ServiceProvider.GetRequiredService<TransientIdProbe>();
            _observations[context.State] = new Observation(scopeProbe.Id, transientProbe.Id);

            // Trigger a nested single-state call during batch evaluation
            await _manager.IsEnabledAsync(_nestedState);

            return true;
        }
    }
}
