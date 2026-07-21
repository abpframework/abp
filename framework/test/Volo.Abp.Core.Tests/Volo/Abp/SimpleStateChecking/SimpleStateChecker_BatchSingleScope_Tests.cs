using System;
using System.Globalization;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.SimpleStateChecking;

/// <summary>
/// Tests that batch IsEnabledAsync evaluates non-batch state checkers correctly
/// when reusing a single DI scope (instead of creating N scopes via InternalIsEnabledAsync).
/// </summary>
public class SimpleStateChecker_BatchSingleScope_Tests : SimpleStateCheckerTestBase
{
    [Fact]
    public async Task Batch_Should_Evaluate_NonBatch_Checkers_Correctly()
    {
        var enabled = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        enabled.AddSimpleStateChecker(new MySimpleStateChecker());

        var disabled = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture)
        };
        disabled.AddSimpleStateChecker(new MySimpleStateChecker());

        var result = await SimpleStateCheckerManager.IsEnabledAsync(new[] { enabled, disabled });

        result[enabled].ShouldBeTrue();
        result[disabled].ShouldBeFalse();
        enabled.CheckCount.ShouldBe(1);
        disabled.CheckCount.ShouldBe(1);
    }

    [Fact]
    public async Task Batch_Should_Skip_NonBatch_Check_When_BatchChecker_Already_Disabled()
    {
        // Entity disabled by batch checker should not have non-batch checker invoked
        var entity = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture) // fails batch checker
        };
        entity.AddSimpleStateChecker(new MySimpleBatchStateChecker());
        entity.AddSimpleStateChecker(new MySimpleStateChecker());

        var result = await SimpleStateCheckerManager.IsEnabledAsync(new[] { entity });

        result[entity].ShouldBeFalse();
        entity.MultipleCheckCount.ShouldBe(1); // batch checker was called
        entity.CheckCount.ShouldBe(0); // non-batch checker was NOT called (skipped because batch disabled it)
    }

    [Fact]
    public async Task Batch_Should_Handle_Mix_Of_Entities_With_And_Without_Checkers()
    {
        var noChecker = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };

        var withChecker = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        withChecker.AddSimpleStateChecker(new MySimpleStateChecker());

        var failingChecker = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture)
        };
        failingChecker.AddSimpleStateChecker(new MySimpleStateChecker());

        var result = await SimpleStateCheckerManager.IsEnabledAsync(
            new[] { noChecker, withChecker, failingChecker });

        result[noChecker].ShouldBeTrue();
        result[withChecker].ShouldBeTrue();
        result[failingChecker].ShouldBeFalse();

        noChecker.CheckCount.ShouldBe(0);
        withChecker.CheckCount.ShouldBe(1);
        failingChecker.CheckCount.ShouldBe(1);
    }

    [Fact]
    public async Task Batch_Should_Handle_Large_Number_Of_Entities()
    {
        var entities = new MyStateEntity[1000];
        for (int i = 0; i < 1000; i++)
        {
            entities[i] = new MyStateEntity
            {
                CreationTime = i % 2 == 0
                    ? DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
                    : DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture)
            };
            entities[i].AddSimpleStateChecker(new MySimpleStateChecker());
        }

        var result = await SimpleStateCheckerManager.IsEnabledAsync(entities);

        for (int i = 0; i < 1000; i++)
        {
            result[entities[i]].ShouldBe(i % 2 == 0);
            entities[i].CheckCount.ShouldBe(1);
        }
    }
}
