using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp.Application.Services
{
    /// <summary>
    /// This interface must be implemented by all application services to register and identify them by convention.
    /// </summary>
    public interface IApplicationService : ITransientDependency, IRemoteService, IUnitOfWorkEnabled, IValidationEnabled, IAuthorizationEnabled
    {

    }
}
