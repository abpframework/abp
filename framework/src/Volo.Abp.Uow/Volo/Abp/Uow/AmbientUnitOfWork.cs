using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    [ExposeServices(typeof(IAmbientUnitOfWork), typeof(IUnitOfWorkAccessor))]
    public class AmbientUnitOfWork : IAmbientUnitOfWork, ISingletonDependency
    {
        public IUnitOfWork UnitOfWork => _currentUow.Value;

        private readonly AsyncLocal<IUnitOfWork> _currentUow;

        public AmbientUnitOfWork()
        {
            _currentUow = new AsyncLocal<IUnitOfWork>();
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _currentUow.Value = unitOfWork;
        }
    }
}