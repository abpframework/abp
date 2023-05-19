using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.FeatureManagement;

public abstract class FeatureValueRepository_Tests<TStartupModule> : FeatureManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IFeatureValueRepository Repository { get; set; }

    protected FeatureValueRepository_Tests()
    {
        Repository = GetRequiredService<IFeatureValueRepository>();
    }

    [Fact]
    public async Task FindAsync()
    {
        //feature value does exists

        var featureValue = await Repository.FindAsync(
            TestFeatureDefinitionProvider.ProjectCount,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Enterprise.ToString()
        );

        featureValue.ShouldNotBeNull();
        featureValue.Value.ShouldBe("3");

        //feature value does not exists
        featureValue = await Repository.FindAsync(
            TestFeatureDefinitionProvider.ProjectCount,
            EditionFeatureValueProvider.ProviderName,
            "undefined-edition-id"
        );

        featureValue.ShouldBeNull();
    }

    [Fact]
    public async Task FindAllAsync()
    {
        var featureValues = await Repository.FindAllAsync(
            TestFeatureDefinitionProvider.ProjectCount,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Enterprise.ToString()
        );

        featureValues.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetListAsync()
    {
        var featureValues = await Repository.GetListAsync(
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Enterprise.ToString()
        );

        featureValues.Count.ShouldBeGreaterThan(0);

        featureValues.ShouldContain(
            fv => fv.Name == TestFeatureDefinitionProvider.SocialLogins &&
                  fv.Value == "true"
        );

        featureValues.ShouldContain(
            fv => fv.Name == TestFeatureDefinitionProvider.ProjectCount &&
                  fv.Value == "3"
        );
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var exception = await Record.ExceptionAsync(async () =>
            await Repository.DeleteAsync(TestFeatureDefinitionProvider.SocialLogins, "true"));
        Assert.Null(exception);
    }

    [Fact]
    public async Task DeleteForProviderNameAndKey()
    {
		using (var scope = ServiceProvider.CreateScope())
		{
			var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            using (var uow = uowManager.Begin(new AbpUnitOfWorkOptions()))
            {
		        await Repository.DeleteAsync(TestFeatureDefinitionProvider.SocialLogins, "true");

				await uow.CompleteAsync();
            }
		}

		var features = await Repository.GetListAsync(TestFeatureDefinitionProvider.SocialLogins, "true");
        features.ShouldBeEmpty();
	}
}
