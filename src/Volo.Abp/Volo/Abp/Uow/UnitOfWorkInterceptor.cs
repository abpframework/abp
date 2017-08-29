using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

	    public override void Intercept(IAbpMethodInvocation invocation)
	    {
	        if (!UnitOfWorkHelper.IsUnitOfWorkMethod(invocation.Method))
	        {
				invocation.Proceed();
	            return;
            }

			using (var uow = _unitOfWorkManager.Begin())
			{
				invocation.Proceed();
				uow.Complete();
			}
		}

	    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (!UnitOfWorkHelper.IsUnitOfWorkMethod(invocation.Method))
            {
                invocation.Proceed();
                return;
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                await invocation.ProceedAsync();
                await uow.CompleteAsync();
            }
        }
    }
}
