using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc
{
    public abstract class AbpController : Controller
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IObjectMapper ObjectMapper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);
    }
}
