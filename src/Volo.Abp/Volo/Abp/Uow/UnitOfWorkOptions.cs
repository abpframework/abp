using System.Data;

namespace Volo.Abp.Uow
{
    /// <summary>
    /// Global (default) unit of work options
    /// </summary>
    public class UnitOfWorkOptions
    {
        //TODO: Implement default options!
        public UnitOfWorkTransactionBehavior TransactionBehavior { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }
    }
}