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
    private readonly IFeatureValueRepository _featureValueRepository;
    private ICurrentUser _currentUser;
    private readonly FeatureManagementTestData _testData;


    public FeatureAppService_Tests()
    {
        _featureAppService = GetRequiredService<IFeatureAppService>();
        _featureValueRepository = GetRequiredService<IFeatureValueRepository>();
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

    private void Login(Guid userId)
    {
        _currentUser.Id.Returns(userId);
        _currentUser.IsAuthenticated.Returns(true);
    }
}
