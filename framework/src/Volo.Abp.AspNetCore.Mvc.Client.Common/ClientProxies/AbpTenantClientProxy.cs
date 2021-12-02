// This file is part of AbpTenantClientProxy, you can customize it here
// ReSharper disable once CheckNamespace

using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Pages.Abp.MultiTenancy.ClientProxies
{
    [RemoteService(false)]
    [DisableConventionalRegistration]
    public partial class AbpTenantClientProxy
    {
    }
}
