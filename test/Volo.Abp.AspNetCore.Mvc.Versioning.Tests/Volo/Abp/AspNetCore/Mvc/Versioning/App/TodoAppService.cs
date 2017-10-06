using Volo.Abp.ApiVersioning;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App
{
    public class TodoAppService : ITodoAppService
    {
        private readonly IRequestedApiVersion _requestedApiVersion;

        public TodoAppService(IRequestedApiVersion requestedApiVersion)
        {
            _requestedApiVersion = requestedApiVersion;
        }

        public string Get(int id)
        {
            return id + "-" + GetVersionOrNone();
        }

        private string GetVersionOrNone()
        {
            return _requestedApiVersion.Current ?? "NONE";
        }
    }
}
