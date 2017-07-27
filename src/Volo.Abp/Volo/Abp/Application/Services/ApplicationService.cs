using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Volo.Abp.Application.Services
{
    public abstract class ApplicationService : IApplicationService
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }
        
        public IObjectMapper ObjectMapper { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;
    }
}