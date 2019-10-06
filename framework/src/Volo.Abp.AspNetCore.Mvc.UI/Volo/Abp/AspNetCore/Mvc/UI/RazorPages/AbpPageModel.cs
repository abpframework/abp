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
        public IServiceProvider ServiceProvider { get; set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);
        
        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        public IClock Clock => LazyGetRequiredService(ref _clock);
        private IClock _clock;

        public AlertList Alerts => AlertManager.Alerts;

        public IUnitOfWorkManager UnitOfWorkManager => LazyGetRequiredService(ref _unitOfWorkManager);
        private IUnitOfWorkManager _unitOfWorkManager;

        protected Type ObjectMapperContext { get; set; }
        public IObjectMapper ObjectMapper
        {
            get
            {
                if (_objectMapper != null)
                {
                    return _objectMapper;
                }

                if (ObjectMapperContext == null)
                {
                    return LazyGetRequiredService(ref _objectMapper);
                }

                return LazyGetRequiredService(
                    typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext),
                    ref _objectMapper
                );
            }
        }
        private IObjectMapper _objectMapper;

        public IGuidGenerator GuidGenerator => LazyGetRequiredService(ref _guidGenerator);
        private IGuidGenerator _guidGenerator;

        public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);
        private ILoggerFactory _loggerFactory;

        public IStringLocalizerFactory StringLocalizerFactory => LazyGetRequiredService(ref _stringLocalizerFactory);
        private IStringLocalizerFactory _stringLocalizerFactory;

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

        public ICurrentUser CurrentUser => LazyGetRequiredService(ref _currentUser);
        private ICurrentUser _currentUser;

        public ICurrentTenant CurrentTenant => LazyGetRequiredService(ref _currentTenant);
        private ICurrentTenant _currentTenant;

        public ISettingProvider SettingProvider => LazyGetRequiredService(ref _settingProvider);
        private ISettingProvider _settingProvider;

        public IModelStateValidator ModelValidator => LazyGetRequiredService(ref _modelValidator);
        private IModelStateValidator _modelValidator;

        public IAuthorizationService AuthorizationService => LazyGetRequiredService(ref _authorizationService);
        private IAuthorizationService _authorizationService;

        public IAlertManager AlertManager => LazyGetRequiredService(ref _alertManager);
        private IAlertManager _alertManager;

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
