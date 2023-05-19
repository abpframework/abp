using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features;

public class TestFeatureStore : IFeatureStore, ISingletonDependency
{
    public const string Tenant1IdValue = "f460fcf7-f944-469a-967b-3b2463323dfe";
    public const string Tenant2IdValue = "e10428ad-4608-4c34-a304-6f82502156f2";

    public static Guid Tenant1Id = new Guid(Tenant1IdValue);
    public static Guid Tenant2Id = new Guid(Tenant2IdValue);

    private readonly List<SettingRecord> _settingRecords;

    public TestFeatureStore()
    {
        _settingRecords = new List<SettingRecord>
            {
                new SettingRecord("BooleanTestFeature1", TenantFeatureValueProvider.ProviderName, Tenant1Id.ToString(), "true"),
                new SettingRecord("BooleanTestFeature2", TenantFeatureValueProvider.ProviderName, Tenant1Id.ToString(), "true"),
                new SettingRecord("IntegerTestFeature1", TenantFeatureValueProvider.ProviderName, Tenant2Id.ToString(), "34")
            };
    }

    public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(
            _settingRecords.FirstOrDefault(sr =>
                sr.Name == name &&
                sr.ProviderName == providerName &&
                sr.ProviderKey == providerKey
            )?.Value
        );
    }

    private class SettingRecord
    {
        public string Name { get; }
        public string ProviderName { get; }
        public string ProviderKey { get; }
        public string Value { get; }

        public SettingRecord(string name, string providerName, string providerKey, string value)
        {
            Name = name;
            ProviderName = providerName;
            ProviderKey = providerKey;
            Value = value;
        }
    }
}
