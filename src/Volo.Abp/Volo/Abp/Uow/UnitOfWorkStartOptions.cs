using System;
using System.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkStartOptions
    {
        //Revise this since Begin/BeginReserved accepts different options and that can make confusion!

        public bool RequiresNew { get; set; }

        public string ReservationName { get; set; }

        public bool? IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }
    }

    /// <summary>
    /// Global (default) unit of work options
    /// </summary>
    public class UnitOfWorkOptions
    {
        public IUnitOfWorkTransactionBehavior IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }
    }

    public interface IUnitOfWorkTransactionBehavior
    {
        UnitOfWorkStartOptions Set(UnitOfWorkTransactionBehaviorContext context);
    }

    public class ConventionalUnitOfWorkTransactionBehavior : IUnitOfWorkTransactionBehavior
    {
        public UnitOfWorkStartOptions Set(UnitOfWorkTransactionBehaviorContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitOfWorkTransactionBehaviorContext : IServiceProviderAccessor
    {
        public UnitOfWorkStartOptions Options { get; }
        
        public IServiceProvider ServiceProvider { get; }

        public UnitOfWorkTransactionBehaviorContext(UnitOfWorkStartOptions options, IServiceProvider serviceProvider)
        {
            Options = options;
            ServiceProvider = serviceProvider;
        }
    }
}