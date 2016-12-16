using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Uow
{
    internal class UnitOfWorkInfo
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public IServiceScope ServiceScope { get; set; }

        public UnitOfWorkInfo(IUnitOfWork unitOfWork, IServiceScope serviceScope)
        {
            UnitOfWork = unitOfWork;
            ServiceScope = serviceScope;
        }
    }
}