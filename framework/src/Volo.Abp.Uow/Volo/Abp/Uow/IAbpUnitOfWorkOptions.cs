using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public interface IAbpUnitOfWorkOptions
    {
        bool IsTransactional { get; }

        IsolationLevel? IsolationLevel { get; }

        /// <summary>
        /// Milliseconds
        /// </summary>
        int? Timeout { get; }
    }
}
