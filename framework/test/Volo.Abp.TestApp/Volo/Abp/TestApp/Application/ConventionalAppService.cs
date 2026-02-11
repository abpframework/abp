using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.TestApp.Application;

public class ConventionalAppService : IApplicationService, ITransientDependency
{
    [Authorize]
    [UnitOfWork]
    public virtual Task GetAsync()
    {
        return Task.CompletedTask;
    }
}
