using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.LIbs;

public class InstallLibsService_Tests : AbpCliTestBase
{
    private const string ReactPackageJson = "{ \"dependencies\": { \"react\": \"^19.2.4\" } }";
    private const string VuePackageJson = "{ \"dependencies\": { \"vue\": \"^3.5.0\" } }";

    private ICmdHelper _cmdHelper;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _cmdHelper = Substitute.For<ICmdHelper>();
        // Answers both "npm -v" and "yarn -v".
        _cmdHelper.RunCmdAndGetOutput(default, default(string)).ReturnsForAnyArgs("1.22.22");
        services.AddTransient(provider => _cmdHelper);
    }

    [Theory]
    [InlineData("package-lock.json", "npm install --ignore-scripts")]
    [InlineData("pnpm-lock.yaml", "pnpm install --ignore-scripts")]
    [InlineData("yarn.lock", "yarn --ignore-scripts")]
    public async Task Should_Install_React_Project_With_The_Package_Manager_It_Declares(
        string lockFile, string expectedCommand)
    {
        using var project = new TemporaryJavaScriptProject(ReactPackageJson, lockFile);

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd(expectedCommand, project.Directory);
    }

    [Fact]
    public async Task Should_Install_React_Project_With_Npm_When_It_Declares_No_Package_Manager()
    {
        using var project = new TemporaryJavaScriptProject(ReactPackageJson);

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd("npm install --ignore-scripts", project.Directory);
        _cmdHelper.DidNotReceive().RunCmd("yarn --ignore-scripts", Arg.Any<string>());
    }

    [Fact]
    public async Task Should_Keep_Using_Yarn_For_Other_JavaScript_Frameworks_Without_A_Lock_File()
    {
        using var project = new TemporaryJavaScriptProject(VuePackageJson);

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd("yarn --ignore-scripts", project.Directory);
    }

    [Fact]
    public async Task Should_Keep_Using_Yarn_For_Other_JavaScript_Frameworks()
    {
        // The project has an npm lock file on purpose: only React projects follow the declared package manager.
        using var project = new TemporaryJavaScriptProject(VuePackageJson, "package-lock.json");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd("yarn --ignore-scripts", project.Directory);
        _cmdHelper.DidNotReceive().RunCmd("npm install --ignore-scripts", Arg.Any<string>());
    }
}
