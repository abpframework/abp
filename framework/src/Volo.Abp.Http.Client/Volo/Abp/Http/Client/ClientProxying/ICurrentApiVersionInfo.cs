using System;

namespace Volo.Abp.Http.Client.ClientProxying;

public interface ICurrentApiVersionInfo
{
    ApiVersionInfo ApiVersionInfo { get; }

    IDisposable Change(ApiVersionInfo apiVersionInfo);
}
