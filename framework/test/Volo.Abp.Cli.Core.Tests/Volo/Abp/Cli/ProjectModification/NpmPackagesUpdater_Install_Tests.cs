using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.Cli.Utils;
using Volo.Abp.Threading;
using Xunit;

namespace Volo.Abp.Cli.ProjectModification;

/// <summary>
/// Covers which package manager installs the dependencies after "abp update" changed a package.json.
/// </summary>
public class NpmPackagesUpdater_Install_Tests : AbpCliTestBase
{
    private const string ReactPackageJson =
        "{ \"dependencies\": { \"react\": \"^19.2.4\", \"@volo/abp-react-app-config\": \"~10.7.0\" } }";

    private const string VuePackageJson =
        "{ \"dependencies\": { \"vue\": \"^3.5.0\", \"@volo/abp-app-config\": \"~10.7.0\" } }";

    private ICmdHelper _cmdHelper;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _cmdHelper = Substitute.For<ICmdHelper>();
        // Answers both "npm -v" and "yarn -v".
        _cmdHelper.RunCmdAndGetOutput(default, default(string)).ReturnsForAnyArgs("1.22.22");
        services.AddTransient(provider => _cmdHelper);
        services.AddTransient<AlwaysUpdatingNpmPackagesUpdater>();
    }

    [Fact]
    public async Task Update_Should_Install_React_Project_With_The_Package_Manager_It_Declares()
    {
        using var project = new TemporaryJavaScriptProject(ReactPackageJson, "vite.config.ts", "package-lock.json");

        await GetRequiredService<AlwaysUpdatingNpmPackagesUpdater>().Update(project.Directory);

        _cmdHelper.Received(1).RunCmd("npm install --ignore-scripts", project.Directory);
        _cmdHelper.DidNotReceive().RunCmd("yarn --ignore-scripts", Arg.Any<string>());
    }

    [Fact]
    public async Task Update_Should_Keep_Using_Yarn_For_Other_JavaScript_Frameworks()
    {
        // The project has an npm lock file on purpose: only React projects follow the declared package manager.
        using var project = new TemporaryJavaScriptProject(VuePackageJson, "vite.config.ts", "package-lock.json");

        await GetRequiredService<AlwaysUpdatingNpmPackagesUpdater>().Update(project.Directory);

        _cmdHelper.Received().RunCmd("yarn --ignore-scripts", Arg.Any<string>());
        _cmdHelper.DidNotReceive().RunCmd("npm install --ignore-scripts", Arg.Any<string>());
    }

    /// <summary>
    /// Skips the version lookup on the NPM registry and reports every package.json as updated.
    /// </summary>
    private class AlwaysUpdatingNpmPackagesUpdater : NpmPackagesUpdater
    {
        public AlwaysUpdatingNpmPackagesUpdater(
            PackageJsonFileFinder packageJsonFileFinder,
            NpmGlobalPackagesChecker npmGlobalPackagesChecker,
            ICancellationTokenProvider cancellationTokenProvider,
            IInstallLibsService installLibsService,
            ICmdHelper cmdHelper,
            NpmHelper npmHelper)
            : base(
                packageJsonFileFinder,
                npmGlobalPackagesChecker,
                cancellationTokenProvider,
                installLibsService,
                cmdHelper,
                npmHelper)
        {
        }

        protected override Task<bool> UpdatePackagesInFile(
            string filePath,
            bool includePreviews = false,
            bool includeReleaseCandidates = false,
            bool switchToStable = false,
            string specifiedVersion = null,
            string specifiedLeptonXVersion = null,
            bool includePreRc = false)
        {
            return Task.FromResult(true);
        }
    }
}
