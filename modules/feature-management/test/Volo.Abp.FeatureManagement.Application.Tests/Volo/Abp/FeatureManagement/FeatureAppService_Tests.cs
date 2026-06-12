using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Features;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.FeatureManagement;

public class FeatureAppService_Tests : FeatureManagementApplicationTestBase
{
    private readonly IFeatureAppService _featureAppService;
    private ICurrentUser _currentUser;
    private readonly FeatureManagementTestData _testData;


    public FeatureAppService_Tests()
    {
        _featureAppService = GetRequiredService<IFeatureAppService>();
        _testData = GetRequiredService<FeatureManagementTestData>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task GetAsync()
    {
        Login(_testData.User1Id);

        var featureList = await _featureAppService.GetAsync(EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());

        featureList.ShouldNotBeNull();
        featureList.Groups.SelectMany(g => g.Features).ShouldContain(feature => feature.Name == TestFeatureDefinitionProvider.SocialLogins);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        Login(_testData.User1Id);

        await _featureAppService.UpdateAsync(EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString(), new UpdateFeaturesDto()
            {
                Features = new List<UpdateFeatureDto>()
                {
                        new UpdateFeatureDto()
                        {
                            Name = TestFeatureDefinitionProvider.SocialLogins,
                            Value = false.ToString().ToLowerInvariant()
                        }
                }
            });

        (await _featureAppService.GetAsync(EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).Groups.SelectMany(g => g.Features).Any(x =>
                x.Name == TestFeatureDefinitionProvider.SocialLogins &&
                x.Value == false.ToString().ToLowerInvariant())
            .ShouldBeTrue();
    }

    [Fact]
    public async Task Select_Child_Feature_Should_Also_Update_Parent_Feature()
    {
        Login(_testData.User1Id);

        await _featureAppService.UpdateAsync(EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString(), new UpdateFeaturesDto()
            {
                Features = new List<UpdateFeatureDto>()
                {
                    new UpdateFeatureDto()
                    {
                        Name = TestFeatureDefinitionProvider.EmailSupport,
                        Value = true.ToString().ToLowerInvariant()
                    },
                    new UpdateFeatureDto()
                    {
                        Name = TestFeatureDefinitionProvider.EmailSupportMaxNumber,
                        Value = true.ToString().ToLowerInvariant()
                    }
                }
            });

        (await _featureAppService.GetAsync(EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).Groups.SelectMany(g => g.Features).Any(x =>
                x.Name == TestFeatureDefinitionProvider.EmailSupportMaxNumber &&
                x.Value == true.ToString().ToLowerInvariant())
            .ShouldBeTrue();

        (await _featureAppService.GetAsync(EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).Groups.SelectMany(g => g.Features).Any(x =>
                x.Name == TestFeatureDefinitionProvider.EmailSupport &&
                x.Value == true.ToString().ToLowerInvariant())
            .ShouldBeTrue();
    }

    [Fact]
    public async Task ResetToDefaultAsync()
    {
        Login(_testData.User1Id);
        var exception = await Record.ExceptionAsync(async () =>
            await _featureAppService.DeleteAsync("test", "test"));
        Assert.Null(exception);
    }

    [Fact]
    public async Task GetAsync_Should_Not_Return_Features_With_Disallowed_Provider()
    {
        Login(_testData.User1Id);

        var editionFeatures = await _featureAppService.GetAsync(
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());

        editionFeatures.Groups.SelectMany(g => g.Features)
            .ShouldNotContain(feature => feature.Name == TestFeatureDefinitionProvider.TenantOnlyFeature);

        var tenantFeatures = await _featureAppService.GetAsync(
            TenantFeatureValueProvider.ProviderName,
            Guid.NewGuid().ToString());

        tenantFeatures.Groups.SelectMany(g => g.Features)
            .ShouldContain(feature => feature.Name == TestFeatureDefinitionProvider.TenantOnlyFeature);
    }

    [Fact]
    public async Task GetAsync_Should_Not_Return_Orphan_Child_When_Parent_Is_Disallowed()
    {
        Login(_testData.User1Id);

        var editionFeatures = await _featureAppService.GetAsync(
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());

        var featureNames = editionFeatures.Groups.SelectMany(g => g.Features).Select(f => f.Name).ToList();
        featureNames.ShouldNotContain(TestFeatureDefinitionProvider.TenantOnlyParentFeature);
        featureNames.ShouldNotContain(TestFeatureDefinitionProvider.OrphanChildOfTenantOnly);

        var tenantFeatures = await _featureAppService.GetAsync(
            TenantFeatureValueProvider.ProviderName,
            Guid.NewGuid().ToString());

        var tenantNames = tenantFeatures.Groups.SelectMany(g => g.Features).Select(f => f.Name).ToList();
        tenantNames.ShouldContain(TestFeatureDefinitionProvider.TenantOnlyParentFeature);
        tenantNames.ShouldContain(TestFeatureDefinitionProvider.OrphanChildOfTenantOnly);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_Exception_For_Disallowed_Provider()
    {
        Login(_testData.User1Id);

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _featureAppService.UpdateAsync(
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString(),
                new UpdateFeaturesDto
                {
                    Features = new List<UpdateFeatureDto>
                    {
                        new UpdateFeatureDto
                        {
                            Name = TestFeatureDefinitionProvider.TenantOnlyFeature,
                            Value = true.ToString().ToLowerInvariant()
                        }
                    }
                });
        });
    }

    private void Login(Guid userId)
    {
        _currentUser.Id.Returns(userId);
        _currentUser.IsAuthenticated.Returns(true);
    }
}
