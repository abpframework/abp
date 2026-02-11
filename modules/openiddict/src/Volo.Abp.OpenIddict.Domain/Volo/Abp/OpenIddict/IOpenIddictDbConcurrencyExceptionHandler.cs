using System.Threading.Tasks;
using Volo.Abp.Data;

namespace Volo.Abp.OpenIddict;

public interface IOpenIddictDbConcurrencyExceptionHandler
{
    Task HandleAsync(AbpDbConcurrencyException exception);
}
