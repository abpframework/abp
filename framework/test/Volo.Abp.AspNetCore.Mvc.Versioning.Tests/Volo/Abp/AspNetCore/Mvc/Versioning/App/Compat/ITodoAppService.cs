using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App.Compat
{
    public interface ITodoAppService : IApplicationService
    {
        string Get(int id);
    }
}