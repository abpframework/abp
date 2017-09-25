using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkStartOptions : IUnitOfWorkStartOptions
    {
        /// <summary>
        /// Default: false.
        /// </summary>
        public bool IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }

        public UnitOfWorkStartOptions Clone()
        {
            return new UnitOfWorkStartOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout
            };
        }
    }
}