using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.SimpleStateChecking;

public class SimpleStateChecker_Tests : SimpleStateCheckerTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpSimpleStateCheckerOptions<MyStateEntity>>(options =>
        {
            options.GlobalStateCheckers.Add<MyGlobalSimpleStateChecker>();
            options.GlobalStateCheckers.Add<MyGlobalSimpleBatchStateChecker>();
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task State_Check_Should_Be_Works()
    {
        var myStateEntity = new MyStateEntity()
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture),
            LastModificationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };
        myStateEntity.AddSimpleStateChecker(new MySimpleStateChecker());

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeTrue();

        myStateEntity.CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeFalse();
    }

    [Fact]
    public async Task Global_State_Check_Should_Be_Works()
    {
        var myStateEntity = new MyStateEntity()
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeFalse();

        myStateEntity.LastModificationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeTrue();
    }

    [Fact]
    public async Task Multiple_State_Check_Should_Be_Works()
    {
        var checker = new MySimpleBatchStateChecker();

        var myStateEntities = new MyStateEntity[]
        {
                new MyStateEntity()
                {
                    CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture),
                    LastModificationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
                },

                new MyStateEntity()
                {
                    CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture),
                    LastModificationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
                }
        };

        foreach (var myStateEntity in myStateEntities)
        {
            myStateEntity.AddSimpleStateChecker(checker);
        }

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntities)).ShouldAllBe(x => x.Value);

        foreach (var myStateEntity in myStateEntities)
        {
            myStateEntity.CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);
        }

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntities)).ShouldAllBe(x => !x.Value);
    }

    [Fact]
    public async Task Multiple_Global_State_Check_Should_Be_Works()
    {
        var myStateEntity = new MyStateEntity()
        {
            CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
        };

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeFalse();

        myStateEntity.LastModificationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);

        (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeTrue();
    }
}
