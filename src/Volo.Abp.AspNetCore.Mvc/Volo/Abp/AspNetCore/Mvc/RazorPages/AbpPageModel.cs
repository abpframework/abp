using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.RazorPages
{
    public abstract class AbpPageModel : PageModel
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IObjectMapper ObjectMapper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public IModelStateValidator ModelValidator { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;
        
        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        protected virtual NoContentResult NoContent() //TODO: Is that true to return empty result like that?
        {
            return new NoContentResult();
        }

        protected virtual void ValidateModel()
        {
            ModelValidator?.Validate(ModelState);
        }
    }
}
