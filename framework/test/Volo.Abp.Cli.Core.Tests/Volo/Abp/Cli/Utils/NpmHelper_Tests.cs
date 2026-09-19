using NSubstitute;
using Shouldly;
using Xunit;

namespace Volo.Abp.Cli.Utils;

public class NpmHelper_Tests
{
    [Theory]
    [InlineData("1.22.22", "--ignore-scripts")]
    [InlineData("4.12.0", "--mode=skip-build")]
    [InlineData("! Corepack is about to download https://repo.yarnpkg.com/4.12.0/packages/yarnpkg-cli/bin/yarn.js\r\n4.12.0", "--mode=skip-build")]
    [InlineData("2.4.3", "--mode=skip-build")]
    [InlineData("npm error npx canceled due to missing packages and no YES option: [\"yarn@1.22.22\"]", "--ignore-scripts")]
    [InlineData("", "--ignore-scripts")]
    public void GetYarnIgnoreScriptsOption(string yarnVersionOutput, string expectedOption)
    {
        var cmdHelper = Substitute.For<ICmdHelper>();
        cmdHelper.RunCmdAndGetOutput("npx --no -- yarn -v", "project-dir").Returns(yarnVersionOutput);

        new NpmHelper(cmdHelper).GetYarnIgnoreScriptsOption("project-dir").ShouldBe(expectedOption);
    }
}
