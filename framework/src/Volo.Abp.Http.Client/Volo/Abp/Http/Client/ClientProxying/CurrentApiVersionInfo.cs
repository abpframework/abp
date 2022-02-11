using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.ClientProxying;

public class CurrentApiVersionInfo : ICurrentApiVersionInfo, ISingletonDependency
{
    public ApiVersionInfo ApiVersionInfo => _currentApiVersionInfo.Value;

    private readonly AsyncLocal<ApiVersionInfo> _currentApiVersionInfo = new AsyncLocal<ApiVersionInfo>();

    public virtual IDisposable Change(ApiVersionInfo apiVersionInfo)
    {
        var parent = ApiVersionInfo;
        _currentApiVersionInfo.Value = apiVersionInfo;
        return new DisposeAction(() =>
        {
            _currentApiVersionInfo.Value = parent;
        });
    }
}
