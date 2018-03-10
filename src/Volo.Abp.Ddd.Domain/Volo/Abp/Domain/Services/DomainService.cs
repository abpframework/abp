using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Guids;

namespace Volo.Abp.Domain.Services
{
    public abstract class DomainService : IDomainService
    {
        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);
        
        protected DomainService()
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
        }
    }
}