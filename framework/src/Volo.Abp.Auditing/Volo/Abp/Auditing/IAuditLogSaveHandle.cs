using System;
using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    public interface IAuditLogSaveHandle : IDisposable
    {
        Task SaveAsync();
        void AddException(Exception exception);
    }
}