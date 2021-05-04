using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;

namespace Volo.Abp.SimpleStateChecking
{
    public abstract class SimpleStateCheckerTestBase : AbpIntegratedTest<AbpTestModule>
    {
        protected readonly ISimpleStateCheckerManager<MyStateEntity> SimpleStateCheckerManager;

        public SimpleStateCheckerTestBase()
        {
            SimpleStateCheckerManager = GetRequiredService<ISimpleStateCheckerManager<MyStateEntity>>();
        }

        public class MyStateEntity : IHasSimpleStateCheckers<MyStateEntity>
        {
            public int CheckCount { get; set; }

            public int GlobalCheckCount { get; set; }

            public int MultipleCheckCount { get; set; }

            public int MultipleGlobalCheckCount { get; set; }

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

        public class MySimpleStateChecker : ISimpleStateChecker<SimpleStateCheckerTestBase.MyStateEntity>
        {
            public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<SimpleStateCheckerTestBase.MyStateEntity> context)
            {
                context.State.CheckCount += 1;
                return Task.FromResult(context.State.CreationTime > DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture));
            }
        }

        public class MyGlobalSimpleStateChecker : ISimpleStateChecker<SimpleStateCheckerTestBase.MyStateEntity>, ITransientDependency
        {
            public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<SimpleStateCheckerTestBase.MyStateEntity> context)
            {
                context.State.GlobalCheckCount += 1;
                return Task.FromResult(context.State.LastModificationTime.HasValue);
            }
        }

        public class MySimpleBatchStateChecker : SimpleBatchStateCheckerBase<SimpleStateCheckerTestBase.MyStateEntity>
        {
            public override Task<SimpleStateCheckerResult<SimpleStateCheckerTestBase.MyStateEntity>> IsEnabledAsync(SimpleBatchStateCheckerContext<SimpleStateCheckerTestBase.MyStateEntity> context)
            {
                foreach (var state in context.States)
                {
                    state.MultipleCheckCount += 1;
                }

                var result = new SimpleStateCheckerResult<SimpleStateCheckerTestBase.MyStateEntity>(context.States);

                foreach (var x in result)
                {
                    result[x.Key] = x.Key.CreationTime > DateTime.Parse("2020-01-01", CultureInfo.InvariantCulture);
                }

                return Task.FromResult(result);
            }
        }

        public class MyGlobalSimpleBatchStateChecker : SimpleBatchStateCheckerBase<SimpleStateCheckerTestBase.MyStateEntity>, ITransientDependency
        {
            public override Task<SimpleStateCheckerResult<SimpleStateCheckerTestBase.MyStateEntity>> IsEnabledAsync(SimpleBatchStateCheckerContext<SimpleStateCheckerTestBase.MyStateEntity> context)
            {
                foreach (var state in context.States)
                {
                    state.MultipleGlobalCheckCount += 1;
                }

                var result = new SimpleStateCheckerResult<SimpleStateCheckerTestBase.MyStateEntity>(context.States);

                foreach (var x in result)
                {
                    result[x.Key] = x.Key.LastModificationTime.HasValue;
                }

                return Task.FromResult(result);
            }
        }
    }
}
