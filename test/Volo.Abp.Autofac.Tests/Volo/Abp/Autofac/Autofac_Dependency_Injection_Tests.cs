using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Autofac
{
    public class Autofac_DependencyInjection_Standard_Tests : DependencyInjection_Standard_Tests
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
