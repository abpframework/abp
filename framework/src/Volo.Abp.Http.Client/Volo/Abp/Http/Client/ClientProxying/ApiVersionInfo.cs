using System;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ApiVersionInfo  //TODO: Rename to not conflict with api versioning apis
{
    public string BindingSource { get; }
    public string Version { get; }

    public ApiVersionInfo(string bindingSource, string version)
    {
        BindingSource = bindingSource;
        Version = version;
    }

    public bool ShouldSendInQueryString()
    {
        //TODO: Constant! TODO: Other sources!
        return !BindingSource.IsIn("Path");
    }
}
