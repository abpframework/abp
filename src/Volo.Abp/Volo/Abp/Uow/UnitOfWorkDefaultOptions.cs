using System;
using System.Data;
using System.Reflection;

namespace Volo.Abp.Uow
{
    //TODO: Implement default options!
    
    /// <summary>
    /// Global (default) unit of work options
    /// </summary>
    public class UnitOfWorkDefaultOptions
    {
        public UnitOfWorkTransactionBehavior TransactionBehavior { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }

        internal UnitOfWorkOptions Normalize(UnitOfWorkOptions options)
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

        internal bool CalculateIsTransactional(bool autoValue)
        {
            switch (TransactionBehavior)
            {
                case UnitOfWorkTransactionBehavior.Enabled:
                    return true;
                case UnitOfWorkTransactionBehavior.Disabled:
                    return false;
                case UnitOfWorkTransactionBehavior.Auto:
                    return autoValue;
                default:
                    throw new AbpException("Not implemented TransactionBehavior value: " + TransactionBehavior);
            }
        }
    }
}