using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;
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

        public async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            //TODO: Check UOW attribute and other conditions!

            using (var uow = _unitOfWorkManager.Begin())
            {
                await invocation.ProceedAsync();
                await uow.CompleteAsync();
            }
        }
    }
}
