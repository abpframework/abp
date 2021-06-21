using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.UI.RazorPages
{
    public abstract class AbpPageModel : PageModel
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        [Obsolete("Use LazyServiceProvider instead.")]
        public IServiceProvider ServiceProvider { get; set; }

        protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

        protected AlertList Alerts => AlertManager.Alerts;

        protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

        protected Type ObjectMapperContext { get; set; }
        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
            ObjectMapperContext == null
                ? provider.GetRequiredService<IObjectMapper>()
                : (IObjectMapper) provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));

        protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

        protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

        protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

        protected IStringLocalizer L
        {
            get
            {
                if (_localizer == null)
                {
                    _localizer = CreateLocalizer();
                }

                return _localizer;
            }
        }

        private IStringLocalizer _localizer;

        protected Type LocalizationResourceType { get; set; }

        protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

        protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

        protected ISettingProvider SettingProvider => LazyServiceProvider.LazyGetRequiredService<ISettingProvider>();

        protected IModelStateValidator ModelValidator => LazyServiceProvider.LazyGetRequiredService<IModelStateValidator>();

        protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

        protected IAlertManager AlertManager => LazyServiceProvider.LazyGetRequiredService<IAlertManager>();

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);

        protected IAppUrlProvider AppUrlProvider => LazyServiceProvider.LazyGetRequiredService<IAppUrlProvider>();

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

        protected virtual IStringLocalizer CreateLocalizer()
        {
            if (LocalizationResourceType != null)
            {
                return StringLocalizerFactory.Create(LocalizationResourceType);
            }

            var localizer = StringLocalizerFactory.CreateDefaultOrNull();
            if (localizer == null)
            {
                throw new AbpException($"Set {nameof(LocalizationResourceType)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
            }

            return localizer;
        }

        protected RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
        {
            return Redirect(GetRedirectUrl(returnUrl, returnUrlHash));
        }

        protected virtual string GetRedirectUrl(string returnUrl, string returnUrlHash = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (!returnUrlHash.IsNullOrWhiteSpace())
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return returnUrl;
        }

        private string NormalizeReturnUrl(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                return GetAppHomeUrl();
            }

            if (Url.IsLocalUrl(returnUrl) || AppUrlProvider.IsRedirectAllowedUrl(returnUrl))
            {
                return returnUrl;
            }

            return GetAppHomeUrl();
        }

        protected virtual string GetAppHomeUrl()
        {
            return "~/"; //TODO: ???
        }
    }
}
