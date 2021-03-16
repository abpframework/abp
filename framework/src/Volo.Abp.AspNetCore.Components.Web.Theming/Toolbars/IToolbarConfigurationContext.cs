using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars
{
    public interface IToolbarConfigurationContext : IServiceProviderAccessor
    {
        Toolbar Toolbar { get; }

        IAuthorizationService AuthorizationService { get; }

        IStringLocalizerFactory StringLocalizerFactory { get; }

        Task<bool> IsGrantedAsync(string policyName);

        [CanBeNull]
        IStringLocalizer GetDefaultLocalizer();

        [NotNull]
        public IStringLocalizer GetLocalizer<T>();

        [NotNull]
        public IStringLocalizer GetLocalizer(Type resourceType);
    }
}
