using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AgentTools.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AgentTools;

public class AbpAgentToolsModuleTests : AbpIntegratedTest<AbpAgentToolsTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Theory]
    [InlineData("en", "Enable agent tools")]
    [InlineData("tr", "Ajan araçlarını etkinleştir")]
    public async Task Feature_And_Error_Texts_Resolve_In_The_Culture(string culture, string featureName)
    {
        using (CultureHelper.Use(culture))
        {
            var factory = GetRequiredService<IStringLocalizerFactory>();
            var feature = await GetRequiredService<IFeatureDefinitionManager>().GetAsync(AgentToolsFeatures.Enable);
            feature.DefaultValue.ShouldBe("true");
            feature.DisplayName.Localize(factory).Value.ShouldBe(featureName);
            feature.Description!.Localize(factory).ResourceNotFound.ShouldBeFalse();

            var localizer = factory.Create(typeof(AbpAgentToolsResource));
            var codes = typeof(AgentToolsErrorCodes).GetFields().Select(f => (string)f.GetRawConstantValue()!).ToList();
            codes.ShouldNotBeEmpty();
            codes.ShouldAllBe(code => !localizer[code].ResourceNotFound);
        }
    }

    [Fact]
    public void Names_That_Applications_And_Clients_Use_Do_Not_Change()
    {
        AgentToolsFeatures.Enable.ShouldBe("AgentTools.Enable");
        AbpAgentToolPolicyOptions.ConfigurationSection.ShouldBe("AgentTools:Tools");
        typeof(AgentToolsErrorCodes).GetFields().Select(f => (f.Name, (string)f.GetRawConstantValue()!)).ShouldBe(
        [
            ("ToolNotFound", "AgentTools:ToolNotFound"),
            ("ToolDisabled", "AgentTools:ToolDisabled"),
            ("ApprovalNotSupported", "AgentTools:ApprovalNotSupported"),
            ("ApprovalPolicyChanged", "AgentTools:ApprovalPolicyChanged"),
            ("FeatureDisabled", "AgentTools:FeatureDisabled")
        ], ignoreOrder: true);
    }

    [Fact]
    public void Error_Codes_Map_To_The_Resource()
    {
        GetRequiredService<IOptions<AbpExceptionLocalizationOptions>>().Value.ErrorCodeNamespaceMappings["AgentTools"]
            .ShouldBe(typeof(AbpAgentToolsResource));
    }
}
