using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.TestBase;

namespace Volo.Abp.Identity
{
    public class AbpIdentityApplicationTestBase : AbpIntegratedTest<AbpIdentityApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
