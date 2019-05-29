using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{
    public class IdentityServerTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
