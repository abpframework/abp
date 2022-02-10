using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.ClientProxying;

public class CurrentApiVersionInfo : ICurrentApiVersionInfo, ITransientDependency
{
    public ApiVersionInfo ApiVersionInfo => _currentPrincipal.Value;

    private readonly AsyncLocal<ApiVersionInfo> _currentPrincipal = new AsyncLocal<ApiVersionInfo>();

    public virtual IDisposable Change(ApiVersionInfo apiVersionInfo)
    {
        var parent = ApiVersionInfo;
        _currentPrincipal.Value = apiVersionInfo;
        return new DisposeAction(() =>
        {
            _currentPrincipal.Value = parent;
        });
    }
}
