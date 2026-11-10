using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.Account.Web;

[DependsOn(
    typeof(AbpAccountApplicationTestModule),
    typeof(AbpAccountWebModule)
)]
public class AbpAccountWebTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(Substitute.For<IWebHostEnvironment>());
        context.Services.Replace(ServiceDescriptor.Singleton(Substitute.For<IAuthenticationService>()));
        context.Services.Replace(ServiceDescriptor.Singleton<ISettingStore, TestSettingStore>());
    }
}

public class TestSettingStore : ISettingStore
{
    public Dictionary<string, string> GlobalValues { get; } = new();

    public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(GetOrNull(name, providerName));
    }

    public Task<List<SettingValue>> GetAllAsync(string[] names, string providerName, string providerKey)
    {
        return Task.FromResult(names.Select(name => new SettingValue(name, GetOrNull(name, providerName))).ToList());
    }

    private string GetOrNull(string name, string providerName)
    {
        return providerName == GlobalSettingValueProvider.ProviderName ? GlobalValues.GetOrDefault(name) : null;
    }
}
