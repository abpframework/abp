// This file is part of AbpApplicationConfigurationClientProxy, you can customize it here
// ReSharper disable once CheckNamespace

using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies
{
    [RemoteService(false)]
    [DisableConventionalRegistration]
    public partial class AbpApplicationConfigurationClientProxy
    {
    }
}
