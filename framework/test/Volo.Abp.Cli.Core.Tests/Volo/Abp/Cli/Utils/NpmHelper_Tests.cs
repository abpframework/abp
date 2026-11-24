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

    [Theory]
    [InlineData(null, new string[0], NodePackageManager.Npm)]
    [InlineData(null, new[] { "package-lock.json" }, NodePackageManager.Npm)]
    [InlineData(null, new[] { "npm-shrinkwrap.json" }, NodePackageManager.Npm)]
    [InlineData(null, new[] { "yarn.lock" }, NodePackageManager.Yarn)]
    [InlineData(null, new[] { "pnpm-lock.yaml" }, NodePackageManager.Pnpm)]
    [InlineData("yarn@4.12.0", new string[0], NodePackageManager.Yarn)]
    [InlineData("pnpm@9.12.0+sha512.4abf7", new[] { "package-lock.json" }, NodePackageManager.Pnpm)]
    [InlineData("npm@11.4.2", new[] { "yarn.lock" }, NodePackageManager.Npm)]
    [InlineData("bun@1.2.0", new[] { "yarn.lock" }, NodePackageManager.Yarn)]
    public void DetectPackageManager(string packageManagerField, string[] lockFiles, NodePackageManager expected)
    {
        var packageJson = packageManagerField == null
            ? "{ \"name\": \"my-app\" }"
            : $"{{ \"name\": \"my-app\", \"packageManager\": \"{packageManagerField}\" }}";

        using var project = new TemporaryJavaScriptProject(packageJson, lockFiles);

        new NpmHelper(Substitute.For<ICmdHelper>()).DetectPackageManager(project.Directory).ShouldBe(expected);
    }

    [Fact]
    public void DetectPackageManager_Should_Use_Lock_File_When_Package_Json_Cannot_Be_Read()
    {
        using var project = new TemporaryJavaScriptProject("{ this is not json", "pnpm-lock.yaml");

        new NpmHelper(Substitute.For<ICmdHelper>()).DetectPackageManager(project.Directory).ShouldBe(NodePackageManager.Pnpm);
    }

    [Theory]
    [InlineData("package-lock.json", "1.22.22", "npm install @volo/abp-react-app-config@~10.7.1 --ignore-scripts")]
    [InlineData("pnpm-lock.yaml", "1.22.22", "pnpm add @volo/abp-react-app-config@~10.7.1 --ignore-scripts")]
    [InlineData("yarn.lock", "1.22.22", "yarn add @volo/abp-react-app-config@~10.7.1 --ignore-scripts")]
    [InlineData("yarn.lock", "4.12.0", "yarn add @volo/abp-react-app-config@~10.7.1 --mode=skip-build")]
    public void GetAddPackageCommand(string lockFile, string yarnVersion, string expectedCommand)
    {
        using var project = new TemporaryJavaScriptProject("{ \"name\": \"my-app\" }", lockFile);
        var cmdHelper = Substitute.For<ICmdHelper>();
        cmdHelper.RunCmdAndGetOutput("yarn -v", project.Directory).Returns(yarnVersion);

        new NpmHelper(cmdHelper)
            .GetAddPackageCommand(project.Directory, "@volo/abp-react-app-config", "~10.7.1")
            .ShouldBe(expectedCommand);
    }

    [Fact]
    public void GetAddPackageCommand_Should_Not_Add_Version_When_It_Is_Not_Given()
    {
        using var project = new TemporaryJavaScriptProject("{ \"name\": \"my-app\" }", "package-lock.json");

        new NpmHelper(Substitute.For<ICmdHelper>())
            .GetAddPackageCommand(project.Directory, "@volo/abp-react-app-config")
            .ShouldBe("npm install @volo/abp-react-app-config --ignore-scripts");
    }

    [Theory]
    [InlineData(null, new[] { "package-lock.json" }, "npm install --ignore-scripts")]
    [InlineData(null, new[] { "pnpm-lock.yaml" }, "pnpm install --ignore-scripts")]
    [InlineData(null, new[] { "yarn.lock" }, "yarn --ignore-scripts")]
    [InlineData("npm@11.4.2", new string[0], "npm install --ignore-scripts")]
    [InlineData("yarn@1.22.22", new string[0], "yarn --ignore-scripts")]
    [InlineData(null, new string[0], "npm install --ignore-scripts")]
    public void GetInstallCommand(string packageManagerField, string[] lockFiles, string expectedCommand)
    {
        var packageJson = packageManagerField == null
            ? "{ \"name\": \"my-app\" }"
            : $"{{ \"name\": \"my-app\", \"packageManager\": \"{packageManagerField}\" }}";

        using var project = new TemporaryJavaScriptProject(packageJson, lockFiles);
        var cmdHelper = Substitute.For<ICmdHelper>();
        cmdHelper.RunCmdAndGetOutput("yarn -v", project.Directory).Returns("1.22.22");

        new NpmHelper(cmdHelper).GetInstallCommand(project.Directory).ShouldBe(expectedCommand);
    }

    [Theory]
    [InlineData("@volo/abp-react-app-config && calc.exe", "1.0.0")]
    [InlineData("@volo/abp-react-app-config", "1.0.0 && calc.exe")]
    public void GetAddPackageCommand_Should_Reject_Unsafe_Input(string package, string version)
    {
        using var project = new TemporaryJavaScriptProject("{ \"name\": \"my-app\" }", "package-lock.json");

        Should.Throw<CliUsageException>(() =>
            new NpmHelper(Substitute.For<ICmdHelper>()).GetAddPackageCommand(project.Directory, package, version));
    }
}
