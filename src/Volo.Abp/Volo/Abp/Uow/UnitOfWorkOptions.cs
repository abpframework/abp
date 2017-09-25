using System;
using System.Data;

namespace Volo.Abp.Uow
{
    //TODO: Implement default options!
    
    /// <summary>
    /// Global (default) unit of work options
    /// </summary>
    public class UnitOfWorkOptions
    {
        public UnitOfWorkTransactionBehavior TransactionBehavior { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }

        internal UnitOfWorkStartOptions Normalize(UnitOfWorkStartOptions options)
        {
            if (options.IsolationLevel == null)
            {
                options.IsolationLevel = IsolationLevel;
            }

            if (options.Timeout == null)
            {
                options.Timeout = Timeout;
            }

            return options;
        }
    }
}