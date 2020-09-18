﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.UI.Navigation.Urls
{
    public class AppUrlProvider : IAppUrlProvider, ITransientDependency
    {
        public const string TenantIdPlaceHolder = "{{tenantId}}";
        public const string TenantNamePlaceHolder = "{{tenantName}}";

        protected AppUrlOptions Options { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ITenantStore TenantStore { get; }

        public AppUrlProvider(
            IOptions<AppUrlOptions> options,
            ICurrentTenant currentTenant,
            ITenantStore tenantStore)
        {
            CurrentTenant = currentTenant;
            TenantStore = tenantStore;
            Options = options.Value;
        }

        public virtual async Task<string> GetUrlAsync(string appName, string urlName = null)
        {
            return await ReplacePlaceHoldersAsync(
                await GetConfiguredUrl(
                    appName,
                    urlName
                )
            );
        }

        protected virtual Task<string> GetConfiguredUrl(string appName, string urlName)
        {
            var app = Options.Applications[appName];

            if (urlName.IsNullOrEmpty())
            {
                if (app.RootUrl.IsNullOrEmpty())
                {
                    throw new AbpException(
                        $"RootUrl for the application '{appName}' was not configured. Use {nameof(AppUrlOptions)} to configure it!"
                    );
                }

                return Task.FromResult(app.RootUrl);
            }

            var url = app.Urls.GetOrDefault(urlName);
            if (url.IsNullOrEmpty())
            {
                throw new AbpException(
                    $"Url, named '{urlName}', for the application '{appName}' was not configured. Use {nameof(AppUrlOptions)} to configure it!"
                );
            }

            if (app.RootUrl == null)
            {
                return Task.FromResult(url);
            }

            return Task.FromResult(app.RootUrl.EnsureEndsWith('/') + url);
        }

        protected virtual async Task<string> ReplacePlaceHoldersAsync(string url)
        {
            url = url.Replace(
                TenantIdPlaceHolder,
                CurrentTenant.Id.HasValue ? CurrentTenant.Id.Value.ToString() : ""
            );

            if (!url.Contains(TenantNamePlaceHolder))
            {
                return url;
            }

            var tenantNamePlaceHolder = TenantNamePlaceHolder;

            if (url.Contains(TenantNamePlaceHolder + '.'))
            {
                tenantNamePlaceHolder = TenantNamePlaceHolder + '.';
            }

            if (url.Contains(tenantNamePlaceHolder))
            {
                if (CurrentTenant.Id.HasValue)
                {
                    url = url.Replace(tenantNamePlaceHolder, await GetCurrentTenantNameAsync());
                }
                else
                {
                    url = url.Replace(tenantNamePlaceHolder, "");
                }
            }

            return url;
        }

        private async Task<string> GetCurrentTenantNameAsync()
        {
            if (CurrentTenant.Id.HasValue && CurrentTenant.Name.IsNullOrEmpty())
            {
                var tenantConfiguration = await TenantStore.FindAsync(CurrentTenant.Id.Value);
                return tenantConfiguration.Name;
            }

            return CurrentTenant.Name;
        }
    }
}
