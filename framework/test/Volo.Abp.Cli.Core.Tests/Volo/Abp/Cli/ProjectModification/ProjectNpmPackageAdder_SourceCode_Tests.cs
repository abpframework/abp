using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.ProjectModification;

/// <summary>
/// Covers "abp add-package ... --with-source-code" for NPM packages. The download itself is the same for
/// every package type; only Angular packages are registered in the project afterwards.
/// </summary>
public class ProjectNpmPackageAdder_SourceCode_Tests : AbpCliTestBase
{
    private const string ReactPackageName = "@volo/abp-react-oidc-auth";
    private const string AngularPackageName = "@volo/abp.ng.identity";

    private const string ReactProjectPackageJson =
        "{ \"dependencies\": { \"react\": \"^19.2.4\", \"@volo/abp-app-config\": \"~10.7.1\" } }";

    private const string AngularJson = "{ \"projects\": {} }";

    private ICmdHelper _cmdHelper;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _cmdHelper = Substitute.For<ICmdHelper>();
        _cmdHelper.RunCmdAndGetOutput(default, default(string)).ReturnsForAnyArgs("1.22.22");
        services.AddTransient(provider => _cmdHelper);
        services.AddTransient<SourceCodeRecordingProjectNpmPackageAdder>();
    }

    [Fact]
    public async Task Should_Download_Source_Code_Of_React_Package_Without_Registering_It()
    {
        using var project = CreateProjectWithAngularConfigurationFiles();
        var adder = GetRequiredService<SourceCodeRecordingProjectNpmPackageAdder>();

        await adder.AddNpmPackageAsync(project.Directory, ReactPackage, version: "10.7.1", withSourceCode: true);

        _cmdHelper.Received(1).RunCmd(
            "npm install @volo/abp-react-oidc-auth@10.7.1 --ignore-scripts", out Arg.Any<int>(), project.Directory);
        adder.Downloads.ShouldHaveSingleItem().ShouldBe((project.Directory, ReactPackageName, "10.7.1"));

        // The Angular registration would create tsconfig.prod.json and add a project to angular.json.
        File.Exists(Path.Combine(project.Directory, "tsconfig.prod.json")).ShouldBeFalse();
        File.ReadAllText(Path.Combine(project.Directory, "angular.json")).ShouldBe(AngularJson);
    }

    [Fact]
    public async Task Should_Still_Register_Source_Code_Of_Angular_Package()
    {
        using var project = CreateProjectWithAngularConfigurationFiles();
        var adder = GetRequiredService<SourceCodeRecordingProjectNpmPackageAdder>();

        await adder.AddNpmPackageAsync(
            project.Directory,
            new NpmPackageInfo { Name = AngularPackageName, ApplicationType = NpmApplicationType.Angular },
            withSourceCode: true);

        adder.Downloads.ShouldHaveSingleItem().PackageName.ShouldBe(AngularPackageName);
        File.Exists(Path.Combine(project.Directory, "tsconfig.prod.json")).ShouldBeTrue();
        File.ReadAllText(Path.Combine(project.Directory, "angular.json")).ShouldContain("volo-abp.ng.identity");
    }

    [Fact]
    public async Task Should_Download_Latest_Source_Code_Of_React_Package_When_Version_Is_Not_Given()
    {
        using var project = new TemporaryJavaScriptProject(ReactProjectPackageJson, "package-lock.json");
        var adder = GetRequiredService<SourceCodeRecordingProjectNpmPackageAdder>();

        await adder.AddNpmPackageAsync(project.Directory, ReactPackage, withSourceCode: true);

        // The package is installed with the ABP version range of the project, but a range is not a
        // source code version: like for the other package types, no version is passed to the download.
        _cmdHelper.Received(1).RunCmd(
            "npm install @volo/abp-react-oidc-auth@~10.7.1 --ignore-scripts", out Arg.Any<int>(), project.Directory);
        adder.Downloads.ShouldHaveSingleItem().Version.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Download_Source_Code_Of_React_Package_That_Is_Already_Installed()
    {
        using var project = new TemporaryJavaScriptProject(
            "{ \"dependencies\": { \"@volo/abp-react-oidc-auth\": \"~10.7.1\" } }", "package-lock.json");
        var adder = GetRequiredService<SourceCodeRecordingProjectNpmPackageAdder>();

        await adder.AddNpmPackageAsync(project.Directory, ReactPackage, withSourceCode: true);

        _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default, out _, default);
        adder.Downloads.ShouldHaveSingleItem().PackageName.ShouldBe(ReactPackageName);
    }

    [Fact]
    public async Task Should_Not_Fail_When_Source_Code_Of_React_Package_Is_Not_Downloadable()
    {
        using var project = new TemporaryJavaScriptProject(ReactProjectPackageJson, "package-lock.json");
        var adder = GetRequiredService<SourceCodeRecordingProjectNpmPackageAdder>();
        adder.IsDownloadable = false;

        await adder.AddNpmPackageAsync(project.Directory, ReactPackage, withSourceCode: true);

        adder.Downloads.ShouldHaveSingleItem();
        Directory.Exists(Path.Combine(project.Directory, "projects")).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Download_Source_Code_Of_React_Package_When_It_Is_Not_Requested()
    {
        using var project = new TemporaryJavaScriptProject(ReactProjectPackageJson, "package-lock.json");
        var adder = GetRequiredService<SourceCodeRecordingProjectNpmPackageAdder>();

        await adder.AddNpmPackageAsync(project.Directory, ReactPackage);

        adder.Downloads.ShouldBeEmpty();
    }

    private static NpmPackageInfo ReactPackage => new()
    {
        Name = ReactPackageName,
        ApplicationType = NpmApplicationType.React
    };

    /// <summary>
    /// A project that also has the two files the Angular registration writes to, so a test can tell
    /// whether that registration ran.
    /// </summary>
    private static TemporaryJavaScriptProject CreateProjectWithAngularConfigurationFiles()
    {
        var project = new TemporaryJavaScriptProject(ReactProjectPackageJson, "package-lock.json");

        File.WriteAllText(Path.Combine(project.Directory, "tsconfig.json"), "{ \"compilerOptions\": {} }");
        File.WriteAllText(Path.Combine(project.Directory, "angular.json"), AngularJson);

        return project;
    }
}
