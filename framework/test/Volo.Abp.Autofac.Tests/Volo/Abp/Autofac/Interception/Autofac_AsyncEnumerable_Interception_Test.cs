using System;
using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Autofac.Interception;

public class Autofac_AsyncEnumerable_Interception_Test : AbpAsyncEnumerableInterceptionTestBase<AutofacTestModule>
{
    protected override Task<Action<AbpApplicationCreationOptions>> SetAbpApplicationCreationOptionsAsync()
    {
        return Task.FromResult<Action<AbpApplicationCreationOptions>>(options => options.UseAutofac());
    }
}
