using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Account;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.Account.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAccountAppService), typeof(AccountClientProxy))]
    public partial class AccountClientProxy : ClientProxyBase<IAccountAppService>, IAccountAppService
    {
    }
}
