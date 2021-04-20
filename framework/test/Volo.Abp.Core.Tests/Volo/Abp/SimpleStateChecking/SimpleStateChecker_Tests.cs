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
                options.GlobalSimpleStateCheckers.Add<MyGlobalSimpleStateChecker>();
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
            myStateEntity.AddSimpleStateChecker(new MySimpleStateChecker());

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

    public class MySimpleStateChecker : ISimpleStateChecker<MyStateEntity>
    {
        public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<MyStateEntity> context)
        {
            return Task.FromResult(context.State.CreationTime > DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture));
        }
    }

    public class MyGlobalSimpleStateChecker : ISimpleStateChecker<MyStateEntity>, ITransientDependency
    {
        public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<MyStateEntity> context)
        {
            return Task.FromResult(context.State.LastModificationTime.HasValue);
        }
    }
}
