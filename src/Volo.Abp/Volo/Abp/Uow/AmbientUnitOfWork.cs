using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    [ExposeServices(typeof(IAmbientUnitOfWork), typeof(IUnitOfWorkAccessor))]
    public class AmbientUnitOfWork : IAmbientUnitOfWork, ISingletonDependency
    {
        public IUnitOfWork UnitOfWork => _currentUowInfo.Value;

        private readonly AsyncLocal<IUnitOfWork> _currentUowInfo;

        public AmbientUnitOfWork()
        {
            _currentUowInfo = new AsyncLocal<IUnitOfWork>();
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _currentUowInfo.Value = unitOfWork;
        }
    }
}