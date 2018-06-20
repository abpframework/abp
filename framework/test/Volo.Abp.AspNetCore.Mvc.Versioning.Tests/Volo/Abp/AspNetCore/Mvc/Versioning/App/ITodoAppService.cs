using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App
{
    public interface ITodoAppService : IApplicationService
    {
        string Get(int id);
    }
}