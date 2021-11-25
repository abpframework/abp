using Shouldly;
using Volo.Abp.Cli.ProjectModification;
using Xunit;

namespace Volo.Abp.Cli;

public class ProjectVersionParse_Tests
{
    [Fact]
    public void Find_Abp_Version()
    {
        const string csprojContent = "<Project Sdk=\"Microsoft.NET.Sdk\">" +
                                     "<Import Project=\"..\\..\\common.props\" />" +
                                     "<PropertyGroup>" +
                                     "<TargetFramework>net5.0</TargetFramework>" +
                                     "<RootNamespace>Blazoor.EfCore07062034</RootNamespace>" +
                                     "</PropertyGroup>" +
                                     "<ItemGroup>" +
                                     "<ProjectReference Include=\"..\\Blazoor.EfCore07062034.Domain.Shared\\Blazoor.EfCore07062034.Domain.Shared.csproj\" />" +
                                     "</ItemGroup>" +
                                     "<ItemGroup>" +
                                     "< PackageReference    Include  =  \"Volo.Abp.Emailing\"   Version  =  \"4.4.0-rc.1\"  />" +
                                     "<PackageReference  Include= \"Volo.Abp.PermissionManagement.Domain.Identity\"   Version= \"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.IdentityServer.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.PermissionManagement.Domain.IdentityServer\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.BackgroundJobs.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.AuditLogging.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.FeatureManagement.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.SettingManagement.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.BlobStoring.Database.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.Identity.Pro.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.LanguageManagement.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.LeptonTheme.Management.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Saas.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "<PackageReference Include=\"Volo.Abp.TextTemplateManagement.Domain\" Version=\"4.4.0-rc.1\" />" +
                                     "</ItemGroup>" +
                                     "</Project>";

        var success = SolutionPackageVersionFinder.TryParseVersionFromCsprojFile(csprojContent, out var version);
        success.ShouldBe(true);
        version.ShouldBe("4.4.0-rc.1");
    }

    [Fact]
    public void Find_Abp_Semantic_Version()
    {
        const string csprojContent = "<Project Sdk=\"Microsoft.NET.Sdk\">" +
                                     "<Import Project=\"..\\..\\common.props\" />" +
                                     "<PropertyGroup>" +
                                     "<TargetFramework>net5.0</TargetFramework>" +
                                     "<RootNamespace>Blazoor.EfCore07062034</RootNamespace>" +
                                     "</PropertyGroup>" +
                                     "<ItemGroup>" +
                                     "<ProjectReference Include=\"..\\Blazoor.EfCore07062034.Domain.Shared\\Blazoor.EfCore07062034.Domain.Shared.csproj\" />" +
                                     "</ItemGroup>" +
                                     "<ItemGroup>" +
                                     "< PackageReference  Include =  \"Volo.Abp.Emailing\"   Version=  \"12.8.3-beta.1\"  />" +
                                     "</ItemGroup>" +
                                     "</Project>";

        var success = SolutionPackageVersionFinder.TryParseSemanticVersionFromCsprojFile(csprojContent, out var version);
        success.ShouldBe(true);
        version.Major.ShouldBe(12);
        version.Minor.ShouldBe(8);
        version.Patch.ShouldBe(3);
        version.Release.ShouldBe("beta.1");
    }

}
