using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Autofac.Interception
{
    public class Autofac_Interception_Test : AbpInterceptionTestBase<AutofacTestModule>
    {
		//TODO: Sımplify using autofac in tests!

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildAutofacServiceProvider();
        }
	}
}
