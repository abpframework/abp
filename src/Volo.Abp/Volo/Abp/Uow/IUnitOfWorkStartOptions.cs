using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkStartOptions
    {
        bool IsTransactional { get; }

        IsolationLevel? IsolationLevel { get; }

        TimeSpan? Timeout { get; }
    }
}