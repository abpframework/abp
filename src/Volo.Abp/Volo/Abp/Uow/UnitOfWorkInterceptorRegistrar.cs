using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public static class UnitOfWorkInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (UnitOfWorkHelper.IsUnitOfWorkType(context.ImplementationType.GetTypeInfo()))
            {
                context.Interceptors.Add<UnitOfWorkInterceptor>();
            }
        }
    }
}