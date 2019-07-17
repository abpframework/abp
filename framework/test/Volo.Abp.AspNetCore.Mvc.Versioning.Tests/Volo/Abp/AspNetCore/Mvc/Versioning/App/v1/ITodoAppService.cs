using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App.v1
{
    public interface ITodoAppService : IApplicationService
    {
        string Get(int id);
    }
}