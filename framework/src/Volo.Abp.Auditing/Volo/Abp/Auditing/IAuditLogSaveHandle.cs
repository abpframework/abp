using System;
using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    public interface IAuditLogSaveHandle : IDisposable
    {
        void Save();

        Task SaveAsync();
    }
}