using System.IO;
using Shouldly;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.ProjectModification;

public class ReactProjectFinder_Tests
{
    private const string ReactPackageJson = "{ \"dependencies\": { \"react\": \"^19.2.4\" } }";
    private const string VuePackageJson = "{ \"dependencies\": { \"vue\": \"^3.5.0\" } }";
    private const string MvcPackageJson = "{ \"dependencies\": { \"@abp/aspnetcore.mvc.ui.theme.leptonxlite\": \"~10.7.1\" } }";

    [Fact]
    public void Find_Should_Return_React_Projects_In_Subdirectories()
    {
        using var solution = new TemporaryDirectory();
        var react = solution.CreateFile(Path.Combine("react", "package.json"), ReactPackageJson);
        var reactPublicWeb = solution.CreateFile(Path.Combine("apps", "react-public-web", "package.json"), ReactPackageJson);

        ReactProjectFinder.Find(solution.Path).ShouldBe(new[] { reactPublicWeb, react });
    }

    [Fact]
    public void Find_Should_Return_The_Given_Directory_When_It_Is_A_React_Project()
    {
        using var project = new TemporaryDirectory();
        project.CreateFile("package.json", ReactPackageJson);

        ReactProjectFinder.Find(project.Path).ShouldBe(new[] { project.Path });
    }

    [Fact]
    public void Find_Should_Ignore_Projects_That_Are_Not_React_Projects()
    {
        using var solution = new TemporaryDirectory();
        solution.CreateFile(Path.Combine("vue", "package.json"), VuePackageJson);
        solution.CreateFile(Path.Combine("broken", "package.json"), "{ this is not json");

        // A React dependency next to a .csproj or an angular.json belongs to that project type.
        var web = solution.CreateFile(Path.Combine("src", "Acme.Web", "package.json"), ReactPackageJson);
        File.WriteAllText(Path.Combine(web, "Acme.Web.csproj"), "<Project />");
        var angular = solution.CreateFile(Path.Combine("angular", "package.json"), ReactPackageJson);
        File.WriteAllText(Path.Combine(angular, "angular.json"), "{}");

        solution.CreateFile(Path.Combine("src", "Acme.Mvc", "package.json"), MvcPackageJson);

        ReactProjectFinder.Find(solution.Path).ShouldBeEmpty();
    }

    [Fact]
    public void Find_Should_Not_Search_In_Installed_Packages_And_Build_Outputs()
    {
        using var solution = new TemporaryDirectory();
        var react = solution.CreateFile(Path.Combine("react", "package.json"), ReactPackageJson);
        solution.CreateFile(Path.Combine("react", "node_modules", "some-react-lib", "package.json"), ReactPackageJson);
        solution.CreateFile(Path.Combine("src", "Acme.Host", "bin", "wwwroot", "package.json"), ReactPackageJson);
        solution.CreateFile(Path.Combine("src", "Acme.Host", "obj", "package.json"), ReactPackageJson);

        ReactProjectFinder.Find(solution.Path).ShouldBe(new[] { react });
    }

    [Fact]
    public void Find_Should_Not_Search_In_Template_Folders()
    {
        using var solution = new TemporaryDirectory();
        var react = solution.CreateFile(Path.Combine("react", "package.json"), ReactPackageJson);
        solution.CreateFile(Path.Combine("_templates", "react", "package.json"), ReactPackageJson);

        ReactProjectFinder.Find(solution.Path).ShouldBe(new[] { react });
    }

    [Fact]
    public void Find_Should_Not_Search_Inside_A_React_Project()
    {
        using var solution = new TemporaryDirectory();
        var react = solution.CreateFile(Path.Combine("react", "package.json"), ReactPackageJson);

        // Source code downloaded with "--with-source-code" and a workspace package: both depend on React.
        solution.CreateFile(
            Path.Combine("react", "projects", "volo-abp-react-lowcode", "package.json"), ReactPackageJson);
        solution.CreateFile(Path.Combine("react", "packages", "ui", "package.json"), ReactPackageJson);

        ReactProjectFinder.Find(solution.Path).ShouldBe(new[] { react });
    }
}
