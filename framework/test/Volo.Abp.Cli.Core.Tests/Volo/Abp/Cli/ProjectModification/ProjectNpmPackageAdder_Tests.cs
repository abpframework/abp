using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.ProjectModification;

public class ProjectNpmPackageAdder_Tests : AbpCliTestBase
{
    private ICmdHelper _cmdHelper;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _cmdHelper = Substitute.For<ICmdHelper>();
        _cmdHelper.RunCmdAndGetOutput(default, default(string)).ReturnsForAnyArgs("1.22.22");
        services.AddTransient(provider => _cmdHelper);
    }

    [Fact]
    public async Task RemoveMvcPackageAsync_Should_Reject_Unsafe_Package_Name()
    {
        const string packageName = "@abp/core && calc.exe";
        var directory = CreatePackageJsonDirectory(packageName);

        try
        {
            await Should.ThrowAsync<CliUsageException>(() =>
                GetRequiredService<ProjectNpmPackageAdder>().RemoveMvcPackageAsync(directory, new NpmPackageInfo { Name = packageName }, skipInstallingLibs: true));

            _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task RemoveMvcPackageAsync_Should_Run_Yarn_Remove()
    {
        const string packageName = "@abp/core";
        var directory = CreatePackageJsonDirectory(packageName);

        try
        {
            await GetRequiredService<ProjectNpmPackageAdder>().RemoveMvcPackageAsync(directory, new NpmPackageInfo { Name = packageName }, skipInstallingLibs: true);

            _cmdHelper.Received().RunCmd("yarn remove @abp/core --ignore-scripts");
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    private const string ReactPackageName = "@volo/abp-react-oidc-auth";

    private const string ReactProjectPackageJson =
        "{ \"dependencies\": { \"react\": \"^19.2.4\", \"@volo/abp-app-config\": \"~10.7.1\" } }";

    private static NpmPackageInfo ReactPackage => new()
    {
        Name = ReactPackageName,
        ApplicationType = NpmApplicationType.React
    };

    [Theory]
    [InlineData("package-lock.json", "npm install @volo/abp-react-oidc-auth@~10.7.1 --ignore-scripts")]
    [InlineData("yarn.lock", "yarn add @volo/abp-react-oidc-auth@~10.7.1 --ignore-scripts")]
    [InlineData("pnpm-lock.yaml", "pnpm add @volo/abp-react-oidc-auth@~10.7.1 --ignore-scripts")]
    public async Task AddNpmPackageAsync_Should_Add_React_Package_With_The_Package_Manager_Of_The_Project(
        string lockFile, string expectedCommand)
    {
        using var project = new TemporaryJavaScriptProject(ReactProjectPackageJson, lockFile);

        await GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(project.Directory, ReactPackage);

        _cmdHelper.Received(1).RunCmd(expectedCommand, out Arg.Any<int>(), project.Directory);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Use_Npm_For_React_Package_When_Project_Has_No_Lock_File()
    {
        using var project = new TemporaryJavaScriptProject(ReactProjectPackageJson);

        await GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(project.Directory, ReactPackage);

        _cmdHelper.Received(1).RunCmd(
            "npm install @volo/abp-react-oidc-auth@~10.7.1 --ignore-scripts", out Arg.Any<int>(), project.Directory);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Prefer_Given_Version_For_React_Package()
    {
        using var project = new TemporaryJavaScriptProject(ReactProjectPackageJson, "package-lock.json");

        await GetRequiredService<ProjectNpmPackageAdder>()
            .AddNpmPackageAsync(project.Directory, ReactPackage, version: "10.7.0");

        _cmdHelper.Received(1).RunCmd(
            "npm install @volo/abp-react-oidc-auth@10.7.0 --ignore-scripts", out Arg.Any<int>(), project.Directory);
    }

    [Theory]
    [InlineData("{ \"dependencies\": { \"react\": \"^19.2.4\" } }")]
    [InlineData("{ \"dependencies\": { \"react\": \"^19.2.4\", \"@volo/abp-app-config\": \"file:../../abp/npm/packs/abp-app-config\" } }")]
    public async Task AddNpmPackageAsync_Should_Add_React_Package_Without_Version_When_Project_Has_No_Usable_Abp_Version(
        string packageJson)
    {
        using var project = new TemporaryJavaScriptProject(packageJson, "package-lock.json");

        await GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(project.Directory, ReactPackage);

        _cmdHelper.Received(1).RunCmd(
            "npm install @volo/abp-react-oidc-auth --ignore-scripts", out Arg.Any<int>(), project.Directory);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Not_Add_React_Package_That_Is_Already_Installed()
    {
        using var project = new TemporaryJavaScriptProject(
            "{ \"dependencies\": { \"@volo/abp-react-oidc-auth\": \"~10.7.1\" } }", "package-lock.json");

        await GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(project.Directory, ReactPackage);

        _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default, out _, default);
        _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Fail_When_React_Package_Cannot_Be_Added()
    {
        using var project = new TemporaryJavaScriptProject(ReactProjectPackageJson, "package-lock.json");
        _cmdHelper
            .When(x => x.RunCmd(Arg.Any<string>(), out Arg.Any<int>(), Arg.Any<string>()))
            .Do(callInfo => callInfo[1] = 1);

        var exception = await Should.ThrowAsync<AbpException>(() =>
            GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(project.Directory, ReactPackage));

        exception.Message.ShouldContain(ReactPackageName);
        exception.Message.ShouldContain("exited with code 1");
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Find_The_React_Project_Below_The_Given_Directory()
    {
        using var solution = new TemporaryDirectory();
        var reactDirectory = solution.CreateFile(Path.Combine("react", "package.json"), ReactProjectPackageJson);
        solution.CreateFile(Path.Combine("react", "package-lock.json"));
        solution.CreateFile(
            Path.Combine("src", "Acme.Web", "package.json"),
            "{ \"dependencies\": { \"@abp/aspnetcore.mvc.ui.theme.leptonxlite\": \"~10.7.1\" } }");

        await GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(solution.Path, ReactPackage);

        _cmdHelper.Received(1).RunCmd(
            "npm install @volo/abp-react-oidc-auth@~10.7.1 --ignore-scripts", out Arg.Any<int>(), reactDirectory);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Not_Count_Downloaded_Source_Code_As_Another_React_Project()
    {
        using var solution = new TemporaryDirectory();
        var reactDirectory = solution.CreateFile(Path.Combine("react", "package.json"), ReactProjectPackageJson);
        solution.CreateFile(
            Path.Combine("react", "projects", "volo-abp-react-lowcode", "package.json"),
            "{ \"devDependencies\": { \"react\": \"^19.2.4\" } }");

        await GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(solution.Path, ReactPackage);

        _cmdHelper.Received(1).RunCmd(
            "npm install @volo/abp-react-oidc-auth@~10.7.1 --ignore-scripts", out Arg.Any<int>(), reactDirectory);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Fail_When_The_Directory_Of_React_Package_Does_Not_Exist()
    {
        using var solution = new TemporaryDirectory();
        var missingDirectory = Path.Combine(solution.Path, "missing");

        var exception = await Should.ThrowAsync<CliUsageException>(() =>
            GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(missingDirectory, ReactPackage));

        exception.Message.ShouldContain(missingDirectory);
        exception.Message.ShouldContain("--working-directory");
        _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default, out _, default);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Not_Guess_Between_Several_React_Projects()
    {
        using var solution = new TemporaryDirectory();
        solution.CreateFile(Path.Combine("react", "package.json"), ReactProjectPackageJson);
        solution.CreateFile(Path.Combine("react-public-web", "package.json"), ReactProjectPackageJson);

        var exception = await Should.ThrowAsync<CliUsageException>(() =>
            GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(solution.Path, ReactPackage));

        exception.Message.ShouldContain("--working-directory");
        exception.Message.ShouldContain("  react" + Environment.NewLine);
        exception.Message.ShouldContain("  react-public-web");
        _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default, out _, default);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Fail_When_There_Is_No_React_Project()
    {
        using var solution = new TemporaryDirectory();
        solution.CreateFile(
            Path.Combine("src", "Acme.Web", "package.json"),
            "{ \"dependencies\": { \"@abp/aspnetcore.mvc.ui.theme.leptonxlite\": \"~10.7.1\" } }");

        var exception = await Should.ThrowAsync<CliUsageException>(() =>
            GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(solution.Path, ReactPackage));

        exception.Message.ShouldContain("No React project was found");
        _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default, out _, default);
    }

    [Fact]
    public async Task AddNpmPackageAsync_Should_Keep_Using_Yarn_For_Angular_Package()
    {
        // The project has an npm lock file on purpose: only React packages follow the project's package manager.
        using var project = new TemporaryJavaScriptProject(
            "{ \"dependencies\": { \"@abp/ng.core\": \"~10.7.1\" } }", "package-lock.json");

        await GetRequiredService<ProjectNpmPackageAdder>().AddNpmPackageAsync(
            project.Directory,
            new NpmPackageInfo { Name = "@abp/ng.theme.basic", ApplicationType = NpmApplicationType.Angular });

        _cmdHelper.Received(1).RunCmd("yarn add @abp/ng.theme.basic --ignore-scripts");
        _cmdHelper.DidNotReceiveWithAnyArgs().RunCmd(default, out _, default);
    }

    private static string CreatePackageJsonDirectory(string packageName)
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        File.WriteAllText(
            Path.Combine(directory, "package.json"),
            $"{{ \"dependencies\": {{ \"{packageName}\": \"1.0.0\" }} }}"
        );

        return directory;
    }
}
