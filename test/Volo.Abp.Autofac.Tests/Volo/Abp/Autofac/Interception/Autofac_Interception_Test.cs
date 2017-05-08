using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle.DynamicProxy;

namespace Volo.Abp.Autofac.Interception
{
    public class Autofac_Interception_Test : CastleInterceptionTestBase<AutofacTestModule>
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
