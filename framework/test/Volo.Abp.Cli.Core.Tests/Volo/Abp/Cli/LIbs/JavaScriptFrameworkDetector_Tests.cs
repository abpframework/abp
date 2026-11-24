using System.IO;
using Newtonsoft.Json;
using Shouldly;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.LIbs;

public class JavaScriptFrameworkDetector_Tests
{
    [Theory]
    [InlineData("{ \"dependencies\": { \"react\": \"^19.2.4\" } }", JavaScriptFrameworkType.React)]
    [InlineData("{ \"devDependencies\": { \"react\": \"^19.2.4\" } }", JavaScriptFrameworkType.React)]
    [InlineData("{ \"dependencies\": { \"react\": \"^19.2.4\", \"react-native\": \"0.79.0\" } }", JavaScriptFrameworkType.ReactNative)]
    [InlineData("{ \"dependencies\": { \"react\": \"^19.2.4\", \"next\": \"15.3.0\" } }", JavaScriptFrameworkType.NextJs)]
    [InlineData("{ \"dependencies\": { \"vue\": \"^3.5.0\" } }", JavaScriptFrameworkType.Vue)]
    [InlineData("{ \"dependencies\": { \"@abp/jquery\": \"~10.7.1\" } }", JavaScriptFrameworkType.None)]
    [InlineData("{ \"name\": \"my-app\" }", JavaScriptFrameworkType.None)]
    public void Detect(string packageJson, JavaScriptFrameworkType expected)
    {
        using var project = new TemporaryJavaScriptProject(packageJson);

        JavaScriptFrameworkDetector.Detect(project.PackageJsonFilePath).ShouldBe(expected);
    }

    [Fact]
    public void Detect_Should_Return_None_When_File_Does_Not_Exist()
    {
        var missingFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "package.json");

        JavaScriptFrameworkDetector.Detect(missingFile).ShouldBe(JavaScriptFrameworkType.None);
    }

    [Fact]
    public void Detect_Should_Throw_For_Invalid_Json()
    {
        using var project = new TemporaryJavaScriptProject("{ this is not json");

        Should.Throw<JsonException>(() => JavaScriptFrameworkDetector.Detect(project.PackageJsonFilePath));
    }
}
