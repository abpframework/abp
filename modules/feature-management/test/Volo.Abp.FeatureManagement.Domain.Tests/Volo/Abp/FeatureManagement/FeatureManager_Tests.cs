using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.FeatureManagement;

public class FeatureManager_Tests : FeatureManagementDomainTestBase
{
    private readonly IFeatureManager _featureManager;
    private readonly ICurrentTenant _currentTenant;
    private readonly IFeatureChecker _featureChecker;
    private readonly IFeatureValueRepository _featureValueRepository;

    public FeatureManager_Tests()
    {
        _featureManager = GetRequiredService<IFeatureManager>();
        _featureChecker = GetRequiredService<IFeatureChecker>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _featureValueRepository = GetRequiredService<IFeatureValueRepository>();
    }

    [Fact]
    public async Task Should_Get_A_FeatureValue_For_A_Provider()
    {
        //Default values

        (await _featureManager.GetOrNullDefaultAsync(
            TestFeatureDefinitionProvider.SocialLogins
        )).ShouldBeNull();

        (await _featureManager.GetOrNullDefaultAsync(
            TestFeatureDefinitionProvider.DailyAnalysis
        )).ShouldBe(false.ToString().ToLowerInvariant());

        (await _featureManager.GetOrNullDefaultAsync(
            TestFeatureDefinitionProvider.ProjectCount
        )).ShouldBe("1");

        (await _featureManager.GetOrNullDefaultAsync(
            TestFeatureDefinitionProvider.BackupCount
        )).ShouldBe("0");

        //"Enterprise" edition values

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.SocialLogins,
            TestEditionIds.Enterprise
        )).ShouldBe(true.ToString().ToLowerInvariant());

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.DailyAnalysis,
            TestEditionIds.Enterprise
        )).ShouldBe(false.ToString().ToLowerInvariant());

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.ProjectCount,
            TestEditionIds.Enterprise
        )).ShouldBe("3");

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.BackupCount,
            TestEditionIds.Enterprise
        )).ShouldBe("5");

        //"Ultimate" edition values

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.SocialLogins,
            TestEditionIds.Ultimate
        )).ShouldBe(true.ToString().ToLowerInvariant());

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.DailyAnalysis,
            TestEditionIds.Ultimate
        )).ShouldBe(true.ToString().ToLowerInvariant());

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.ProjectCount,
            TestEditionIds.Ultimate
        )).ShouldBe("10");

        (await _featureManager.GetOrNullForEditionAsync(
            TestFeatureDefinitionProvider.BackupCount,
            TestEditionIds.Ultimate
        )).ShouldBe("10");
    }

    [Fact]
    public async Task Should_Change_Feature_Value_And_Refresh_Cache()
    {
        var tenantId = Guid.NewGuid();

        //It is "False" at the beginning
        using (_currentTenant.Change(tenantId))
        {
            (await _featureChecker.IsEnabledAsync(TestFeatureDefinitionProvider.SocialLogins)).ShouldBeFalse();
        }

        //Set to "True" by host for the tenant
        using (_currentTenant.Change(null))
        {
            (await _featureChecker.IsEnabledAsync(TestFeatureDefinitionProvider.SocialLogins)).ShouldBeFalse();
            await _featureManager.SetForTenantAsync(tenantId, TestFeatureDefinitionProvider.SocialLogins, "True");
            (await _featureManager.GetOrNullForTenantAsync(TestFeatureDefinitionProvider.SocialLogins, tenantId)).ShouldBe("True");
        }

        //Now, it should be "True"
        using (_currentTenant.Change(tenantId))
        {
            (await _featureChecker.IsEnabledAsync(TestFeatureDefinitionProvider.SocialLogins)).ShouldBeTrue();
        }
    }


    [Fact]
    public async Task Should_Get_FeatureValues_With_Provider_For_A_Provider()
    {
        var featureNameValueWithGrantedProviders = await _featureManager.GetAllWithProviderAsync(
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Enterprise.ToString()
        );

        featureNameValueWithGrantedProviders.ShouldContain(x =>
            x.Name == TestFeatureDefinitionProvider.SocialLogins
            && x.Value == true.ToString().ToLowerInvariant() &&
            x.Provider.Name == EditionFeatureValueProvider.ProviderName);

        featureNameValueWithGrantedProviders.ShouldContain(x =>
            x.Name == TestFeatureDefinitionProvider.EmailSupport &&
            x.Value == true.ToString().ToLowerInvariant() &&
            x.Provider.Name == EditionFeatureValueProvider.ProviderName);

        //Default Value
        featureNameValueWithGrantedProviders.ShouldContain(x =>
            x.Name == TestFeatureDefinitionProvider.DailyAnalysis &&
            x.Value == false.ToString().ToLowerInvariant() &&
            x.Provider.Name == DefaultValueFeatureValueProvider.ProviderName);

        featureNameValueWithGrantedProviders.ShouldContain(x =>
            x.Name == TestFeatureDefinitionProvider.UserCount &&
            x.Value == "20" &&
            x.Provider.Name == EditionFeatureValueProvider.ProviderName);

        featureNameValueWithGrantedProviders.ShouldContain(x =>
            x.Name == TestFeatureDefinitionProvider.ProjectCount &&
            x.Value == "3" &&
            x.Provider.Name == EditionFeatureValueProvider.ProviderName);

        featureNameValueWithGrantedProviders.ShouldContain(x =>
            x.Name == TestFeatureDefinitionProvider.BackupCount &&
            x.Value == "5" &&
            x.Provider.Name == EditionFeatureValueProvider.ProviderName);
    }

    [Fact]
    public async Task Test_HandleContextAsync()
    {
        var featureValue = await _featureValueRepository.FindAsync(
            TestFeatureDefinitionProvider.EmailSupport,
            TenantFeatureValueProvider.ProviderName,
            TestEditionIds.TenantId.ToString()
        );

        featureValue.ShouldNotBeNull();
        featureValue.Value.ShouldBe(false.ToString().ToLower());


        featureValue = await _featureValueRepository.FindAsync(
            TestFeatureDefinitionProvider.EmailSupport,
            NextTenantFeatureManagementProvider.ProviderName,
            TestEditionIds.TenantId.ToString()
        );

        featureValue.ShouldNotBeNull();
        featureValue.Value.ShouldBe(true.ToString().ToLower());

        await _featureManager.SetAsync(TestFeatureDefinitionProvider.EmailSupport, true.ToString().ToLower(),
            TenantFeatureValueProvider.ProviderName, TestEditionIds.TenantId.ToString());

        featureValue = await _featureValueRepository.FindAsync(
            TestFeatureDefinitionProvider.EmailSupport,
            TenantFeatureValueProvider.ProviderName,
            TestEditionIds.TenantId.ToString()
        );
        featureValue.ShouldBeNull();

        featureValue = await _featureValueRepository.FindAsync(
            TestFeatureDefinitionProvider.EmailSupport,
            NextTenantFeatureManagementProvider.ProviderName,
            TestEditionIds.TenantId.ToString()
        );
        featureValue.ShouldNotBeNull();
        featureValue.Value.ShouldBe(true.ToString().ToLower());
    }
}
