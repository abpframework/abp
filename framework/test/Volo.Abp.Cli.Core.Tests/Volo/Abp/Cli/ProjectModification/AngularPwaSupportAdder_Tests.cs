using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Shouldly;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.ProjectModification;

public class AngularPwaSupportAdder_Tests : IDisposable
{
    private static readonly string AngularJson = """
        {
          "version": 1,
          "projects": {
            "MyProjectName": {
              "projectType": "application",
              "root": "",
              "architect": {
                "build": {
                  "options": {
                    "polyfills": ["src/polyfills.ts"],
                    "assets": [
                      "src/favicon.ico",
                      "src/assets",
                      {
                        "glob": "web.config",
                        "input": ".",
                        "output": "."
                      }
                    ]
                  }
                },
                "test": {
                  "options": {
                    "assets": ["src/assets"]
                  }
                }
              }
            }
          }
        }
        """.Replace("\r\n", "\n");

    private readonly string _testDirectory;
    private readonly TestAngularPwaSupportAdder _adder;

    public AngularPwaSupportAdder_Tests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "abp-cli-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_testDirectory);

        _adder = new TestAngularPwaSupportAdder(Substitute.For<ICmdHelper>(), new PackageJsonFileFinder());
    }

    public void Dispose()
    {
        Directory.Delete(_testDirectory, recursive: true);
    }

    [Theory]
    [InlineData("{ \"devDependencies\": { \"@angular/cli\": \"~22.0.0\" } }", "22")]
    [InlineData("{ \"devDependencies\": { \"@angular/cli\": \"^21.1.3\" } }", "21")]
    [InlineData("{ \"devDependencies\": { \"@angular/cli\": \"22\" } }", "22")]
    [InlineData("{ \"dependencies\": { \"@angular/cli\": \"~20.3.0\" } }", "20")]
    [InlineData("{ \"dependencies\": { \"@angular/cli\": \"~20.3.0\" }, \"devDependencies\": { \"@angular/cli\": \"~22.0.0\" } }", "22")]
    public void Should_Get_Angular_Cli_Major_Version(string packageJson, string expectedVersion)
    {
        _adder.GetAngularCliMajorVersion(WritePackageJson(packageJson)).ShouldBe(expectedVersion);
    }

    [Theory]
    [InlineData("{ \"devDependencies\": { \"typescript\": \"~5.9.0\" } }")]
    [InlineData("{ \"devDependencies\": { \"@angular/cli\": \"latest\" } }")]
    public void Should_Throw_If_Angular_Cli_Version_Cannot_Be_Determined(string packageJson)
    {
        Should.Throw<AbpException>(() => _adder.GetAngularCliMajorVersion(WritePackageJson(packageJson)));
    }

    [Fact]
    public void Should_Add_Public_Folder_To_Build_Assets()
    {
        Directory.CreateDirectory(Path.Combine(_testDirectory, "public"));

        var result = _adder.AddPublicFolderToBuildAssets(AngularJson, _testDirectory);

        var assets = GetAssets(result, "build");
        assets.Count.ShouldBe(4);
        assets[0].ToString().ShouldBe("src/favicon.ico");
        assets[1].ToString().ShouldBe("src/assets");
        assets[2]["glob"].ToString().ShouldBe("web.config");
        assets[3]["glob"].ToString().ShouldBe("**/*");
        assets[3]["input"].ToString().ShouldBe("public");

        GetAssets(result, "test").Count.ShouldBe(1);
        result.ShouldEndWith("}");
        _adder.AddPublicFolderToBuildAssets(AngularJson + "\n", _testDirectory).ShouldEndWith("}\n");
    }

    [Fact]
    public void Should_Not_Add_Public_Folder_To_Build_Assets_Twice()
    {
        Directory.CreateDirectory(Path.Combine(_testDirectory, "public"));

        var result = _adder.AddPublicFolderToBuildAssets(AngularJson, _testDirectory);

        _adder.AddPublicFolderToBuildAssets(result, _testDirectory).ShouldBe(result);
        GetAssets(result, "build").Count(x => x is JObject && x["input"]?.ToString() == "public").ShouldBe(1);
    }

    [Fact]
    public void Should_Not_Add_Public_Folder_To_Build_Assets_If_It_Does_Not_Exist()
    {
        _adder.AddPublicFolderToBuildAssets(AngularJson, _testDirectory).ShouldBe(AngularJson);
    }

    [Fact]
    public void Should_Add_Public_Folder_Of_Project_Root_To_Build_Assets()
    {
        Directory.CreateDirectory(Path.Combine(_testDirectory, "projects", "app", "public"));
        var angularJson = AngularJson.Replace("\"root\": \"\"", "\"root\": \"projects/app\"");

        var result = _adder.AddPublicFolderToBuildAssets(angularJson, _testDirectory);

        GetAssets(result, "build").Last()["input"].ToString().ShouldBe("projects/app/public");
    }

    [Fact]
    public void Should_Keep_Line_Endings()
    {
        Directory.CreateDirectory(Path.Combine(_testDirectory, "public"));

        var result = _adder.AddPublicFolderToBuildAssets(AngularJson.Replace("\n", "\r\n"), _testDirectory);

        GetAssets(result, "build").Last()["input"].ToString().ShouldBe("public");
        result.ShouldContain("\r\n");
        result.Replace("\r\n", string.Empty).ShouldNotContain("\n");
    }

    [Theory]
    [InlineData("[\"src/favicon.ico\"]", 2)]
    [InlineData("[]", 1)]
    public void Should_Add_Public_Folder_To_Inline_Build_Assets(string assets, int expectedCount)
    {
        Directory.CreateDirectory(Path.Combine(_testDirectory, "public"));
        var angularJson = $"{{ \"projects\": {{ \"app\": {{ \"root\": \"\", \"architect\": {{ \"build\": {{ \"options\": {{ \"assets\": {assets} }} }} }} }} }} }}";

        var result = (JArray)JObject.Parse(_adder.AddPublicFolderToBuildAssets(angularJson, _testDirectory))
            ["projects"]!["app"]!["architect"]!["build"]!["options"]!["assets"];

        result.Count.ShouldBe(expectedCount);
        result.Last()["input"]!.ToString().ShouldBe("public");
    }

    [Fact]
    public void Should_Run_Ng_Add_Through_Npx_And_Add_Public_Folder_To_Build_Assets()
    {
        var cmdHelper = CreateCmdHelper(exitCode: 0);
        WritePackageJson("{ \"devDependencies\": { \"@angular/cli\": \"~22.0.0\" } }");
        var angularJsonPath = Path.Combine(_testDirectory, "angular.json");
        File.WriteAllText(angularJsonPath, AngularJson);

        new AngularPwaSupportAdder(cmdHelper, new PackageJsonFileFinder()).AddPwaSupport(_testDirectory);

        cmdHelper.Received(1).RunCmd(
            "npx -y -p @angular/cli@22 ng add @angular/pwa@22 --skip-confirmation",
            out Arg.Any<int>(),
            _testDirectory + Path.DirectorySeparatorChar);
        GetAssets(File.ReadAllText(angularJsonPath), "build").Last()["input"].ToString().ShouldBe("public");
    }

    [Fact]
    public void Should_Throw_If_Ng_Add_Fails()
    {
        var cmdHelper = CreateCmdHelper(exitCode: 127);
        WritePackageJson("{ \"devDependencies\": { \"@angular/cli\": \"~22.0.0\" } }");
        var angularJsonPath = Path.Combine(_testDirectory, "angular.json");
        File.WriteAllText(angularJsonPath, AngularJson);

        Should.Throw<AbpException>(() => new AngularPwaSupportAdder(cmdHelper, new PackageJsonFileFinder()).AddPwaSupport(_testDirectory));

        File.ReadAllText(angularJsonPath).ShouldBe(AngularJson);
    }

    private ICmdHelper CreateCmdHelper(int exitCode)
    {
        var cmdHelper = Substitute.For<ICmdHelper>();
        cmdHelper
            .When(x => x.RunCmd(Arg.Any<string>(), out Arg.Any<int>(), Arg.Any<string>()))
            .Do(call =>
            {
                call[1] = exitCode;
                Directory.CreateDirectory(Path.Combine(_testDirectory, "public"));
            });
        return cmdHelper;
    }

    private string WritePackageJson(string content)
    {
        var path = Path.Combine(_testDirectory, "package.json");
        File.WriteAllText(path, content);
        return path;
    }

    private static JArray GetAssets(string angularJson, string target)
    {
        return (JArray)JObject.Parse(angularJson)["projects"]!["MyProjectName"]!["architect"]![target]!["options"]!["assets"];
    }

    private class TestAngularPwaSupportAdder : AngularPwaSupportAdder
    {
        public TestAngularPwaSupportAdder(ICmdHelper cmdHelper, PackageJsonFileFinder packageJsonFileFinder)
            : base(cmdHelper, packageJsonFileFinder)
        {
        }

        public new string GetAngularCliMajorVersion(string packageJsonFilePath)
        {
            return base.GetAngularCliMajorVersion(packageJsonFilePath);
        }

        public new string AddPublicFolderToBuildAssets(string angularJson, string workspaceDirectory)
        {
            return base.AddPublicFolderToBuildAssets(angularJson, workspaceDirectory);
        }
    }
}
