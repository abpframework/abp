using Volo.Abp.ApiVersioning;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App.Compat
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
            return "Compat-" + id + "-" + GetVersionOrNone();
        }

        private string GetVersionOrNone()
        {
            return _requestedApiVersion.Current ?? "NONE";
        }
    }
}
