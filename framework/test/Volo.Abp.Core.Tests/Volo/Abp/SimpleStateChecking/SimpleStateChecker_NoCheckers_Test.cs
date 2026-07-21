using System.Globalization;
using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.SimpleStateChecking;

public class SimpleStateChecker_NoCheckers_Test : SimpleStateCheckerTestBase
{
    // No GlobalStateCheckers registered — verifies the fast path when both
    // state.StateCheckers and Options.GlobalStateCheckers are empty.

    [Fact]
    public async Task Entity_With_No_State_Checkers_Should_Be_Enabled()
    {
        var entity = new MyStateEntity();

        (await SimpleStateCheckerManager.IsEnabledAsync(entity)).ShouldBeTrue();
    }

    [Fact]
    public async Task Entity_With_No_State_Checkers_Should_Not_Increment_Check_Counts()
    {
        var entity = new MyStateEntity();

        await SimpleStateCheckerManager.IsEnabledAsync(entity);

        entity.CheckCount.ShouldBe(0);
        entity.GlobalCheckCount.ShouldBe(0);
        entity.MultipleCheckCount.ShouldBe(0);
        entity.MultipleGlobalCheckCount.ShouldBe(0);
    }

    [Fact]
    public async Task Multiple_Entities_With_No_State_Checkers_Should_All_Be_Enabled()
    {
        var entities = new[]
        {
            new MyStateEntity { CreationTime = DateTime.Parse("2022-01-01", CultureInfo.InvariantCulture) },
            new MyStateEntity { CreationTime = DateTime.Parse("2019-01-01", CultureInfo.InvariantCulture) }
        };

        var result = await SimpleStateCheckerManager.IsEnabledAsync(entities);

        result.Values.ShouldAllBe(v => v);
    }

    [Fact]
    public async Task Entity_With_Individual_Checker_Should_Still_Be_Checked()
    {
        var entity = new MyStateEntity
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        entity.AddSimpleStateChecker(new MySimpleStateChecker());

        (await SimpleStateCheckerManager.IsEnabledAsync(entity)).ShouldBeTrue();
        entity.CheckCount.ShouldBe(1);

        entity.CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);

        (await SimpleStateCheckerManager.IsEnabledAsync(entity)).ShouldBeFalse();
        entity.CheckCount.ShouldBe(2);
    }
}
