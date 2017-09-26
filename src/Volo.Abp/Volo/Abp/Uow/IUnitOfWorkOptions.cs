using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkOptions
    {
        bool IsTransactional { get; }

        IsolationLevel? IsolationLevel { get; }

        TimeSpan? Timeout { get; }
    }
}