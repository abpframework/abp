using NSubstitute;
using Shouldly;
using Xunit;

namespace Volo.Abp.Cli.Utils;

public class NpmHelper_Tests
{
    private const string NpxYarnMissing = "npm error npx canceled due to missing packages and no YES option: [\"yarn@1.22.22\"]";

    [Theory]
    [InlineData("1.22.22", null, "yarn --ignore-scripts")]
    [InlineData("4.12.0", null, "yarn --mode=skip-build")]
    [InlineData("! Corepack is about to download https://repo.yarnpkg.com/4.12.0/packages/yarnpkg-cli/bin/yarn.js\r\n4.12.0", null, "yarn --mode=skip-build")]
    [InlineData("/bin/bash: yarn: command not found", "1.22.22", "npx yarn --ignore-scripts")]
    [InlineData("'yarn' is not recognized as an internal or external command,\r\noperable program or batch file.", "4.12.0", "npx yarn --mode=skip-build")]
    [InlineData("/bin/bash: yarn: command not found", NpxYarnMissing, "npx yarn --ignore-scripts")]
    public void GetYarnCommand(string yarnOutput, string npxYarnOutput, string expectedCommand)
    {
        var cmdHelper = Substitute.For<ICmdHelper>();
        cmdHelper.RunCmdAndGetOutput("yarn -v", "project-dir").Returns(yarnOutput);
        cmdHelper.RunCmdAndGetOutput("npx --no -- yarn -v", "project-dir").Returns(npxYarnOutput ?? string.Empty);

        new NpmHelper(cmdHelper).GetYarnCommand("project-dir").ShouldBe(expectedCommand);
    }

    [Fact]
    public void GetYarnCommand_Should_Put_Arguments_Before_Ignore_Scripts_Option()
    {
        var cmdHelper = Substitute.For<ICmdHelper>();
        cmdHelper.RunCmdAndGetOutput("yarn -v", "project-dir").Returns("4.12.0");

        new NpmHelper(cmdHelper).GetYarnCommand("project-dir", "add @abp/core@10.0.0").ShouldBe("yarn add @abp/core@10.0.0 --mode=skip-build");
    }
}
