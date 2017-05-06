using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Modularity;

namespace Volo.Abp.Castle
{
    [DependsOn(typeof(AbpCastleCoreModule))]
    public class AbpCastleCoreTestModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnServiceRegistred(RegisterTestInterceptors);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpCastleCoreTestModule>();
        }

        private static void RegisterTestInterceptors(IOnServiceRegistredArgs registration)
        {
            //TODO: Create an attribute to add interceptors!
            if (typeof(SimpleInterceptionTargetClass) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleInterceptor>();
            }
        }
    }
}
