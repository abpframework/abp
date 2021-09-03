using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.SettingManagement;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.SettingManagement.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IEmailSettingsAppService), typeof(EmailSettingsClientProxy))]
    public partial class EmailSettingsClientProxy : ClientProxyBase<IEmailSettingsAppService>, IEmailSettingsAppService
    {
    }
}
