using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkInterceptor : IAbpInterceptor, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }


        public void Intercept(IAbpMethodInvocation invocation)
        {
            //TODO: Check UOW attribute and other conditions!

            if (invocation.Method.IsAsync())
            {
                PerformAsyncUow(invocation);
            }
            else
            {
                PerformSyncUow(invocation);
            }
        }

        private void PerformSyncUow(IAbpMethodInvocation invocation)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                invocation.Proceed();
                uow.Complete();
            }
        }

        private void PerformAsyncUow(IAbpMethodInvocation invocation)
        {
            var uow = _unitOfWorkManager.Begin();

            try
            {
                invocation.Proceed();
            }
            catch
            {
                uow.Dispose();
                throw;
            }

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    exception => uow.Dispose()
                );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    exception => uow.Dispose()
                );
            }
        }
    }
}
