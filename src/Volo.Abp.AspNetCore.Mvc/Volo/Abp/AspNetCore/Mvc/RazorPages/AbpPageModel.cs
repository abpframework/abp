using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Authorization;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Session;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.RazorPages
{
    public abstract class AbpPageModel : PageModel
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IObjectMapper ObjectMapper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public ICurrentUser CurrentUser { get; set; }

        public ICurrentTenant CurrentTenant { get; set; }

        public IModelStateValidator ModelValidator { get; set; }

        public IAuthorizationService AuthorizationService { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        public IClock Clock { get; set; }

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

        protected virtual Task CheckPolicyAsync(string policyName)
        {
            return AuthorizationService.CheckAsync(policyName);
        }
    }
}
