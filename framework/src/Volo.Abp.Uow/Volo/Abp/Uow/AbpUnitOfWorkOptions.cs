using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public class AbpUnitOfWorkOptions : IAbpUnitOfWorkOptions
    {
        /// <summary>
        /// Default: false.
        /// </summary>
        public bool IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }

        public AbpUnitOfWorkOptions()
        {
            
        }

        public AbpUnitOfWorkOptions(bool isTransactional = false, IsolationLevel? isolationLevel = null, TimeSpan? timeout = null)
        {
            IsTransactional = isTransactional;
            IsolationLevel = isolationLevel;
            Timeout = timeout;
        }

        public AbpUnitOfWorkOptions Clone()
        {
            return new AbpUnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout
            };
        }
    }
}