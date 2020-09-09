﻿using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarConfigurationContext : IToolbarConfigurationContext
    {
        public IServiceProvider ServiceProvider { get; }
        private readonly object _serviceProviderLock = new object();

        private TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (_serviceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        public IAuthorizationService AuthorizationService => LazyGetRequiredService(typeof(IAuthorizationService), ref _authorizationService);
        private IAuthorizationService _authorizationService;

        private IStringLocalizerFactory _stringLocalizerFactory;
        public IStringLocalizerFactory StringLocalizerFactory => LazyGetRequiredService(typeof(IStringLocalizerFactory),ref _stringLocalizerFactory);

        public ITheme Theme { get; }

        public Toolbar Toolbar { get; }

        public ToolbarConfigurationContext(ITheme currentTheme, Toolbar toolbar, IServiceProvider serviceProvider)
        {
            Theme = currentTheme;
            Toolbar = toolbar;
            ServiceProvider = serviceProvider;
        }
        
        public Task<bool> IsGrantedAsync(string policyName)
        {
            return AuthorizationService.IsGrantedAsync(policyName);
        }

        [CanBeNull]
        public IStringLocalizer GetDefaultLocalizer()
        {
            return StringLocalizerFactory.CreateDefaultOrNull();
        }

        [NotNull]
        public IStringLocalizer GetLocalizer<T>()
        {
            return StringLocalizerFactory.Create<T>();
        }
        
        [NotNull]
        public IStringLocalizer GetLocalizer(Type resourceType)
        {
            return StringLocalizerFactory.Create(resourceType);
        }
    }
}
