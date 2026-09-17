using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.SimpleStateChecking;

public class SimpleStateChecker_AndCombineResults_Tests : SimpleStateCheckerTestBase
{
    [Fact]
    public async Task False_Result_From_One_Batch_Checker_Should_Not_Be_Overwritten_By_Later_True_Checker()
    {
        // Regression test for the bug fixed in SimpleStateCheckerManager.IsEnabledAsync:
        // when a state had multiple batch checkers the manager used to OVERWRITE
        // result[x.Key] on each checker, so a later "true" silently masked an earlier "false".
        // The fix AND-combines results.
        //
        // To make sure we actually hit the second batch checker (and not the early-return
        // `result.Values.All(x => !x)` short-circuit), we use two states:
        //   - state1: [falseChecker, trueChecker]  - the target case
        //   - state2: [trueChecker]                - keeps result.Values not all-false
        //                                             so the loop continues to the next checker

        var falseChecker = new AlwaysFalseBatchStateChecker();
        var trueChecker = new AlwaysTrueBatchStateChecker();

        var state1 = new MyStateEntity();
        state1.AddSimpleStateChecker(falseChecker);
        state1.AddSimpleStateChecker(trueChecker);

        var state2 = new MyStateEntity();
        state2.AddSimpleStateChecker(trueChecker);

        var result = await SimpleStateCheckerManager.IsEnabledAsync(new[] { state1, state2 });

        result[state1].ShouldBeFalse();   // before the fix this was True (bug)
        result[state2].ShouldBeTrue();
    }

    [Fact]
    public async Task True_Result_From_One_Batch_Checker_Should_Be_AND_Combined_With_Later_False_Checker()
    {
        // Reverse order: true first then false. Add a second always-true state to keep
        // result.Values not all-false after both checkers ran.
        var trueChecker = new AlwaysTrueBatchStateChecker();
        var falseChecker = new AlwaysFalseBatchStateChecker();

        var state1 = new MyStateEntity();
        state1.AddSimpleStateChecker(trueChecker);
        state1.AddSimpleStateChecker(falseChecker);

        var state2 = new MyStateEntity();
        state2.AddSimpleStateChecker(trueChecker);

        var result = await SimpleStateCheckerManager.IsEnabledAsync(new[] { state1, state2 });

        result[state1].ShouldBeFalse();
        result[state2].ShouldBeTrue();
    }

    [Fact]
    public async Task All_True_Batch_Checkers_Should_Produce_True()
    {
        var trueChecker1 = new AlwaysTrueBatchStateChecker();
        var trueChecker2 = new AlwaysTrueBatchStateChecker();

        var state = new MyStateEntity();
        state.AddSimpleStateChecker(trueChecker1);
        state.AddSimpleStateChecker(trueChecker2);

        var result = await SimpleStateCheckerManager.IsEnabledAsync(new[] { state });

        result[state].ShouldBeTrue();
    }

    public class AlwaysFalseBatchStateChecker : SimpleBatchStateCheckerBase<MyStateEntity>, ITransientDependency
    {
        public override Task<SimpleStateCheckerResult<MyStateEntity>> IsEnabledAsync(SimpleBatchStateCheckerContext<MyStateEntity> context)
        {
            var result = new SimpleStateCheckerResult<MyStateEntity>(context.States);
            foreach (var x in result)
            {
                result[x.Key] = false;
            }
            return Task.FromResult(result);
        }
    }

    public class AlwaysTrueBatchStateChecker : SimpleBatchStateCheckerBase<MyStateEntity>, ITransientDependency
    {
        public override Task<SimpleStateCheckerResult<MyStateEntity>> IsEnabledAsync(SimpleBatchStateCheckerContext<MyStateEntity> context)
        {
            var result = new SimpleStateCheckerResult<MyStateEntity>(context.States);
            foreach (var x in result)
            {
                result[x.Key] = true;
            }
            return Task.FromResult(result);
        }
    }
}
