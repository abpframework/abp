using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace Volo.Abp.OpenIddict.Applications;

public interface IAbpApplicationManager : IOpenIddictApplicationManager
{
    ValueTask<string> GetClientUriAsync(object application, CancellationToken cancellationToken = default);

    ValueTask<string> GetLogoUriAsync(object application, CancellationToken cancellationToken = default);
}
