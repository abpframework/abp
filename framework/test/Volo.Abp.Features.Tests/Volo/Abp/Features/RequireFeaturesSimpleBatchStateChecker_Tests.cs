using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;
using Xunit;

namespace Volo.Abp.Features;

public class RequireFeaturesSimpleBatchStateChecker_Tests : FeatureTestBase
{
    private readonly ISimpleStateCheckerManager<MyStateEntity> _simpleStateCheckerManager;
    private readonly ICurrentTenant _currentTenant;

    public RequireFeaturesSimpleBatchStateChecker_Tests()
    {
        _simpleStateCheckerManager = GetRequiredService<ISimpleStateCheckerManager<MyStateEntity>>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public void Switch_Current_Checker_Test()
    {
        var checker = RequireFeaturesSimpleBatchStateChecker<MyStateEntity2>.Current;
        checker.ShouldNotBeNull();

        RequireFeaturesSimpleBatchStateChecker<MyStateEntity2> checker2 = null;

        using (RequireFeaturesSimpleBatchStateChecker<MyStateEntity2>.Use(new RequireFeaturesSimpleBatchStateChecker<MyStateEntity2>()))
        {
            checker2 = RequireFeaturesSimpleBatchStateChecker<MyStateEntity2>.Current;
            checker2.ShouldNotBeNull();
            checker2.ShouldNotBe(checker);
        }

        checker2.ShouldNotBeNull();
        checker2.ShouldNotBe(checker);
    }

    [Fact]
    public async Task RequireFeaturesSimpleBatchStateChecker_Test()
    {
        // Tenant1: BooleanTestFeature1=true, BooleanTestFeature2=true
        // Tenant2: no boolean features set → false
        using (_currentTenant.Change(TestFeatureStore.Tenant1Id))
        {
            var myStateEntities = new MyStateEntity[]
            {
                new MyStateEntity().RequireFeatures(requiresAll: true, batchCheck: true, "BooleanTestFeature1"),
                new MyStateEntity().RequireFeatures(requiresAll: true, batchCheck: true, "BooleanTestFeature2"),
                new MyStateEntity().RequireFeatures(requiresAll: true, batchCheck: true, "BooleanTestFeature1", "BooleanTestFeature2"),
                new MyStateEntity().RequireFeatures(requiresAll: true, batchCheck: true, "BooleanTestFeature1", "BooleanTestFeature2"),
            };

            var result = await _simpleStateCheckerManager.IsEnabledAsync(myStateEntities);

            result.Count.ShouldBe(myStateEntities.Length);

            result[myStateEntities[0]].ShouldBeTrue();
            result[myStateEntities[1]].ShouldBeTrue();
            result[myStateEntities[2]].ShouldBeTrue();
            result[myStateEntities[3]].ShouldBeTrue();
        }

        using (_currentTenant.Change(TestFeatureStore.Tenant2Id))
        {
            var myStateEntities = new MyStateEntity[]
            {
                new MyStateEntity().RequireFeatures(requiresAll: true, batchCheck: true, "BooleanTestFeature1"),
                new MyStateEntity().RequireFeatures(requiresAll: true, batchCheck: true, "BooleanTestFeature2"),
                new MyStateEntity().RequireFeatures(requiresAll: true, batchCheck: true, "BooleanTestFeature1", "BooleanTestFeature2"),
                new MyStateEntity().RequireFeatures(requiresAll: false, batchCheck: true, "BooleanTestFeature1", "BooleanTestFeature2"),
            };

            var result = await _simpleStateCheckerManager.IsEnabledAsync(myStateEntities);

            result.Count.ShouldBe(myStateEntities.Length);

            result[myStateEntities[0]].ShouldBeFalse();
            result[myStateEntities[1]].ShouldBeFalse();
            result[myStateEntities[2]].ShouldBeFalse();
            result[myStateEntities[3]].ShouldBeFalse();
        }
    }

    [Fact]
    public async Task Current_Should_Not_Be_Null_In_Fresh_ExecutionContext()
    {
        _ = RequireFeaturesSimpleBatchStateChecker<MyStateEntity3>.Current;

        Task<RequireFeaturesSimpleBatchStateChecker<MyStateEntity3>> task;
        using (ExecutionContext.SuppressFlow())
        {
            task = Task.Run(() => RequireFeaturesSimpleBatchStateChecker<MyStateEntity3>.Current);
        }

        var current = await task;
        current.ShouldNotBeNull();
    }

    class MyStateEntity : IHasSimpleStateCheckers<MyStateEntity>
    {
        public List<ISimpleStateChecker<MyStateEntity>> StateCheckers { get; }

        public MyStateEntity()
        {
            StateCheckers = new List<ISimpleStateChecker<MyStateEntity>>();
        }
    }

    class MyStateEntity2 : IHasSimpleStateCheckers<MyStateEntity2>
    {
        public List<ISimpleStateChecker<MyStateEntity2>> StateCheckers { get; }

        public MyStateEntity2()
        {
            StateCheckers = new List<ISimpleStateChecker<MyStateEntity2>>();
        }
    }

    class MyStateEntity3 : IHasSimpleStateCheckers<MyStateEntity3>
    {
        public List<ISimpleStateChecker<MyStateEntity3>> StateCheckers { get; }

        public MyStateEntity3()
        {
            StateCheckers = new List<ISimpleStateChecker<MyStateEntity3>>();
        }
    }
}
