using Volo.Abp.ApiVersioning;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App.v1
{
    public class TodoAppService : ApplicationService, ITodoAppService
    {
        private readonly IRequestedApiVersion _requestedApiVersion;

        public TodoAppService(IRequestedApiVersion requestedApiVersion)
        {
            _requestedApiVersion = requestedApiVersion;
        }

        public string Get(int id)
        {
            return $"Compat-{id}-{GetVersionOrNone()}";
        }

        private string GetVersionOrNone()
        {
            return _requestedApiVersion.Current ?? "NONE";
        }
    }
}
