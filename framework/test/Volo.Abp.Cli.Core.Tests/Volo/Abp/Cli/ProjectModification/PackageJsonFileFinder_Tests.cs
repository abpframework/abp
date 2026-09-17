using System;
using System.IO;
using Shouldly;
using Volo.Abp.Cli.ProjectModification;
using Xunit;

namespace Volo.Abp.Cli;

public class PackageJsonFileFinder_Tests
{
    [Theory]
    [InlineData("Test.csproj", true)]
    [InlineData("angular.json", true)]
    [InlineData("vite.config.ts", true)]
    [InlineData("next.config.ts", true)]
    [InlineData("", false)]
    public void Should_Find_Package_Json_For_Supported_Project_Types(string projectFileName, bool expected)
    {
        var testDirectory = Path.Combine(Path.GetTempPath(), "abp-cli-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(testDirectory);

        try
        {
            var packageJsonPath = Path.Combine(testDirectory, "package.json");
            File.WriteAllText(packageJsonPath, "{}");

            if (!projectFileName.IsNullOrEmpty())
            {
                File.WriteAllText(Path.Combine(testDirectory, projectFileName), string.Empty);
            }

            var result = new PackageJsonFileFinder().Find(testDirectory);

            result.Contains(packageJsonPath).ShouldBe(expected);
        }
        finally
        {
            Directory.Delete(testDirectory, recursive: true);
        }
    }
}
