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
