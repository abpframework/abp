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
                //TODO: Notice that it may add the interceptor more than one for every exposed service type!?
                context.Interceptors.Add<UnitOfWorkInterceptor>();
            }
        }
    }
}