using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkStartOptions
    {
        public bool? IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}