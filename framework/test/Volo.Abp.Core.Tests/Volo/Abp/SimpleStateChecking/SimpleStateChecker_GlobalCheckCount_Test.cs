using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.SimpleStateChecking
{
    public class SimpleStateChecker_GlobalCheckCount_Test : SimpleStateCheckerTestBase
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
        public async Task Simple_State_Check_Should_Be_Prevent_Multiple_Checks()
        {
            var myStateEntities = new SimpleStateCheckerTestBase.MyStateEntity[]
            {
                new SimpleStateCheckerTestBase.MyStateEntity()
                {
                    CreationTime = DateTime.Parse("2022-01-01", CultureInfo.InvariantCulture),
                    LastModificationTime = DateTime.Parse("2022-01-01", CultureInfo.InvariantCulture)
                },

                new SimpleStateCheckerTestBase.MyStateEntity()
                {
                    CreationTime = DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture)
                }
            };

            myStateEntities[0].AddSimpleStateChecker(new MySimpleBatchStateChecker());
            myStateEntities[1].AddSimpleStateChecker(new MySimpleStateChecker());

            (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntities[0])).ShouldBeTrue();

            myStateEntities[0].CheckCount.ShouldBe(0);
            myStateEntities[0].GlobalCheckCount.ShouldBe(1);
            myStateEntities[0].MultipleCheckCount.ShouldBe(1);
            myStateEntities[0].MultipleGlobalCheckCount.ShouldBe(1);

            (await SimpleStateCheckerManager.IsEnabledAsync(myStateEntities[1])).ShouldBeFalse();

            myStateEntities[1].CheckCount.ShouldBe(1);
            myStateEntities[1].GlobalCheckCount.ShouldBe(0);
            myStateEntities[1].MultipleCheckCount.ShouldBe(0);
            myStateEntities[1].MultipleGlobalCheckCount.ShouldBe(0);
        }

        [Fact]
        public async Task Multiple_And_Simple_State_Check_Should_Be_Prevent_Multiple_Checks()
        {
            var myStateEntities = new SimpleStateCheckerTestBase.MyStateEntity[]
            {
                new SimpleStateCheckerTestBase.MyStateEntity()
                {
                    CreationTime = DateTime.Parse("2022-01-01", CultureInfo.InvariantCulture),
                    LastModificationTime = DateTime.Parse("2022-01-01", CultureInfo.InvariantCulture)
                },

                new SimpleStateCheckerTestBase.MyStateEntity()
                {
                    CreationTime = DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture)
                }
            };

            myStateEntities[0].AddSimpleStateChecker(new MySimpleBatchStateChecker());
            myStateEntities[1].AddSimpleStateChecker(new MySimpleStateChecker());

            var result = await SimpleStateCheckerManager.IsEnabledAsync(myStateEntities);
            result.Count.ShouldBe(2);

            result[myStateEntities[0]].ShouldBeTrue();
            result[myStateEntities[1]].ShouldBeFalse();

            myStateEntities[0].CheckCount.ShouldBe(0);
            myStateEntities[0].GlobalCheckCount.ShouldBe(1);
            myStateEntities[0].MultipleCheckCount.ShouldBe(1);
            myStateEntities[0].MultipleGlobalCheckCount.ShouldBe(1);

            myStateEntities[1].CheckCount.ShouldBe(0);
            myStateEntities[1].GlobalCheckCount.ShouldBe(0);
            myStateEntities[1].MultipleCheckCount.ShouldBe(0);
            myStateEntities[1].MultipleGlobalCheckCount.ShouldBe(1);
        }
    }
}
