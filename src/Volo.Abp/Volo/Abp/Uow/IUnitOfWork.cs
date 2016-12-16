using System;
using System.Threading.Tasks;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWork : IDisposable, ITransientDependency
    {
        Task SaveChangesAsync();

        Task CompleteAsync();
    }
}
