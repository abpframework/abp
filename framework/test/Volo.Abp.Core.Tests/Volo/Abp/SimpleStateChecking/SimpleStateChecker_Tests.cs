using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.SimpleStateChecking
{
    public class SimpleStateChecker_Tests : AbpIntegratedTest<AbpTestModule>
    {
        private readonly ISimpleStateCheckerManager<MyStateEntity> _simpleStateCheckerManager;

        public SimpleStateChecker_Tests()
        {
            _simpleStateCheckerManager = GetRequiredService<ISimpleStateCheckerManager<MyStateEntity>>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<AbpSimpleStateCheckerOptions<MyStateEntity>>(options =>
            {
                options.GlobalSimpleStateCheckers.Add<MyGlobalSimpleSingleStateChecker>();
                options.GlobalSimpleStateCheckers.Add<MyGlobalSimpleMultipleStateChecker>();
            });
        }

        [Fact]
        public async Task State_Check_Should_Be_Works()
        {
            var myStateEntity = new MyStateEntity()
            {
                CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture),
                LastModificationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
            };
            myStateEntity.AddSimpleStateChecker(new MySimpleSingleStateChecker());

            (await _simpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeTrue();

            myStateEntity.CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);

            (await _simpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeFalse();
        }

        [Fact]
        public async Task Global_State_Check_Should_Be_Works()
        {
            var myStateEntity = new MyStateEntity()
            {
                CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
            };

            (await _simpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeFalse();

            myStateEntity.LastModificationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);

            (await _simpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeTrue();
        }

        [Fact]
        public async Task Multiple_State_Check_Should_Be_Works()
        {
            var checker = new MySimpleMultipleStateChecker();

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

            //(await _simpleStateCheckerManager.IsEnabledAsync(myStateEntities)).ShouldAllBe(x => x.Value);

            foreach (var myStateEntity in myStateEntities)
            {
                myStateEntity.CreationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);
            }

            (await _simpleStateCheckerManager.IsEnabledAsync(myStateEntities)).ShouldAllBe(x => !x.Value);
        }

        [Fact]
        public async Task Multiple_Global_State_Check_Should_Be_Works()
        {
            var myStateEntity = new MyStateEntity()
            {
                CreationTime = DateTime.Parse("2021-01-01", CultureInfo.InvariantCulture)
            };

            (await _simpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeFalse();

            myStateEntity.LastModificationTime = DateTime.Parse("2001-01-01", CultureInfo.InvariantCulture);

            (await _simpleStateCheckerManager.IsEnabledAsync(myStateEntity)).ShouldBeTrue();
        }
    }

    public class MyStateEntity : IHasSimpleStateCheckers<MyStateEntity>
    {
        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public List<ISimpleStateChecker<MyStateEntity>> SimpleStateCheckers { get; }

        public MyStateEntity()
        {
            SimpleStateCheckers = new List<ISimpleStateChecker<MyStateEntity>>();
        }

        public MyStateEntity AddSimpleStateChecker(ISimpleStateChecker<MyStateEntity> checker)
        {
            SimpleStateCheckers.Add(checker);
            return this;
        }
    }

    public class MySimpleSingleStateChecker : ISimpleSingleStateChecker<MyStateEntity>
    {
        public Task<bool> IsEnabledAsync(SimpleSingleStateCheckerContext<MyStateEntity> context)
        {
            return Task.FromResult(context.State.CreationTime > DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture));
        }
    }

    public class MyGlobalSimpleSingleStateChecker : ISimpleSingleStateChecker<MyStateEntity>, ITransientDependency
    {
        public Task<bool> IsEnabledAsync(SimpleSingleStateCheckerContext<MyStateEntity> context)
        {
            return Task.FromResult(context.State.LastModificationTime.HasValue);
        }
    }

    public class MySimpleMultipleStateChecker : ISimpleMultipleStateChecker<MyStateEntity>
    {
        public Task<SimpleStateCheckerResult<MyStateEntity>> IsEnabledAsync(SimpleMultipleStateCheckerContext<MyStateEntity> context)
        {
            var result = new SimpleStateCheckerResult<MyStateEntity>(context.States);

            foreach (var x in result)
            {
                result[x.Key] = x.Key.CreationTime > DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture);
            }

            return Task.FromResult(result);
        }
    }

    public class MyGlobalSimpleMultipleStateChecker : ISimpleMultipleStateChecker<MyStateEntity>, ITransientDependency
    {
        public Task<SimpleStateCheckerResult<MyStateEntity>> IsEnabledAsync(SimpleMultipleStateCheckerContext<MyStateEntity> context)
        {
            var result = new SimpleStateCheckerResult<MyStateEntity>(context.States);

            foreach (var x in result)
            {
                result[x.Key] = x.Key.LastModificationTime.HasValue;
            }

            return Task.FromResult(result);
        }
    }
}
