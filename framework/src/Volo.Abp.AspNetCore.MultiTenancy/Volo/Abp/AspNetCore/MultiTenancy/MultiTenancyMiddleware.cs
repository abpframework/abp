using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class MultiTenancyMiddleware : IMiddleware, ITransientDependency
    {
        private readonly ITenantConfigurationProvider _tenantConfigurationProvider;
        private readonly ICurrentTenant _currentTenant;

        public MultiTenancyMiddleware(
            ITenantConfigurationProvider tenantConfigurationProvider,
            ICurrentTenant currentTenant)
        {
            _tenantConfigurationProvider = tenantConfigurationProvider;
            _currentTenant = currentTenant;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tenant = await _tenantConfigurationProvider.GetAsync(saveResolveResult: true);
            if (tenant?.Id != _currentTenant.Id)
            {
                using (_currentTenant.Change(tenant?.Id, tenant?.Name))
                {
                    var requestCulture = await TryGetRequestCultureAsync(context);
                    if (requestCulture != null)
                    {
                        CultureInfo.CurrentCulture = requestCulture.Culture;
                        CultureInfo.CurrentUICulture = requestCulture.UICulture;
                        AbpRequestCultureCookieHelper.SetCultureCookie(
                            context,
                            requestCulture
                        );
                    }

                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
        }

        private async Task<RequestCulture> TryGetRequestCultureAsync(HttpContext httpContext)
        {
            var requestCultureFeature = httpContext.Features.Get<IRequestCultureFeature>();

            /* If requestCultureFeature == null, that means the RequestLocalizationMiddleware was not used
             * and we don't want to set the culture. */
            if (requestCultureFeature == null)
            {
                return null;
            }

            /* If requestCultureFeature.Provider is not null, that means RequestLocalizationMiddleware
             * already picked a language, so we don't need to set the default. */
            if (requestCultureFeature.Provider != null)
            {
                return null;
            }

            var settingProvider = httpContext.RequestServices.GetRequiredService<ISettingProvider>();
            var defaultLanguage = await settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
            if (defaultLanguage.IsNullOrWhiteSpace())
            {
                return null;
            }

            string culture;
            string uiCulture;

            if (defaultLanguage.Contains(';'))
            {
                var splitted = defaultLanguage.Split(';');
                culture = splitted[0];
                uiCulture = splitted[1];
            }
            else
            {
                culture = defaultLanguage;
                uiCulture = defaultLanguage;
            }

            return new RequestCulture(culture, uiCulture);
        }
    }
}
