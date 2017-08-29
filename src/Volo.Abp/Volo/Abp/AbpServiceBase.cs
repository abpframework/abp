using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Volo.Abp
{
    public abstract class AbpServiceBase
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IObjectMapper ObjectMapper { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        public ILoggerFactory LoggerFactory { get; set; }

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);
        
        /* Will be added when developed
           - Setting manager
           - Localization manager and helper methods
         */
    }
}
