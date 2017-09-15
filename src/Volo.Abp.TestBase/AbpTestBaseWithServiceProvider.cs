using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.TestBase
{
    public abstract class AbpTestBaseWithServiceProvider
    {
        protected abstract IServiceProvider ServiceProvider { get; }

        protected virtual void WithUnitOfWork(Action action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin())
                {
                    action();

                    uow.Complete();
                }
            }
        }

        protected virtual async Task WithUnitOfWorkAsync(Func<Task> action)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin())
                {
                    await action();

                    await uow.CompleteAsync();
                }
            }
        }

        protected virtual TResult WithUnitOfWork<TResult>(Func<TResult> func)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin())
                {
                    var result = func();
                    uow.Complete();
                    return result;
                }
            }
        }

        protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin())
                {
                    var result = await func();
                    await uow.CompleteAsync();
                    return result;
                }
            }
        }
    }
}