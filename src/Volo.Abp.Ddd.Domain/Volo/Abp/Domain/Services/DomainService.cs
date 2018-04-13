using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace Volo.Abp.Domain.Services
{
    public abstract class DomainService : IDomainService
    {
        public IClock Clock { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }
        
        public ICurrentTenant CurrentTenant { get; set; }

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);
        
        protected DomainService()
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
        }
    }
}