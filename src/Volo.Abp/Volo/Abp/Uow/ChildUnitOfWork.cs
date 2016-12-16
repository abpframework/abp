using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    internal class ChildUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _parent;

        public ChildUnitOfWork([NotNull] IUnitOfWork parent)
        {
            Check.NotNull(parent, nameof(parent));

            _parent = parent;
        }

        public Task SaveChangesAsync()
        {
            return _parent.SaveChangesAsync();
        }

        public Task CompleteAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}