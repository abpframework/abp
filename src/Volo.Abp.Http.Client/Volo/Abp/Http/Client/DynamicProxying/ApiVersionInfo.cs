using System;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    internal class ApiVersionInfo
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
}