using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.UI.RazorPages
{
    public abstract class AbpPageModel : PageModel
    {
        public IClock Clock { get; set; }

        public AlertList Alerts => AlertManager.Alerts;

        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IObjectMapper ObjectMapper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public IStringLocalizerFactory StringLocalizerFactory { get; set; }

        public IStringLocalizer L
        {
            get
            {
                if (_localizer == null)
                {
                    if (LocalizationResourceType == null)
                    {
                        throw new AbpException($"{nameof(LocalizationResourceType)} should be set before using the {nameof(L)} object!");
                    }

                    _localizer = StringLocalizerFactory.Create(LocalizationResourceType);
                }

                return _localizer;
            }
        }
        private IStringLocalizer _localizer;

        protected Type LocalizationResourceType { get; set; }

        public ICurrentUser CurrentUser { get; set; }

        public ICurrentTenant CurrentTenant { get; set; }

        public ISettingProvider SettingProvider { get; set; }

        public IModelStateValidator ModelValidator { get; set; }

        public IAuthorizationService AuthorizationService { get; set; }

        public IAlertManager AlertManager { get; set; }

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

        protected virtual Task CheckPolicyAsync(string policyName)
        {
            return AuthorizationService.CheckAsync(policyName);
        }

        protected virtual PartialViewResult PartialView<TModel>(string viewName, TModel model)
        {
            return new PartialViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<TModel>(ViewData, model),
                TempData = TempData
            };
        }
    }
}
