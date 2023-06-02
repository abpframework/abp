using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict;

public class MongoOpenIddictDbConcurrencyExceptionHandler: IOpenIddictDbConcurrencyExceptionHandler, ITransientDependency
{
    public virtual Task HandleAsync(AbpDbConcurrencyException exception)
    {
        return Task.CompletedTask;
    }
}
