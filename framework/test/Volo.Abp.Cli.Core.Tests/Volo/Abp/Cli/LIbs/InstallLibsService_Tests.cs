using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Cli.Utils;
using Xunit;

namespace Volo.Abp.Cli.LIbs;

public class InstallLibsService_Tests : AbpCliTestBase
{
    private const string ReactPackageJson = "{ \"dependencies\": { \"react\": \"^19.2.4\" } }";
    private const string VuePackageJson = "{ \"dependencies\": { \"vue\": \"^3.5.0\" } }";

    private ICmdHelper _cmdHelper;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _cmdHelper = Substitute.For<ICmdHelper>();
        // Answers both "npm -v" and "yarn -v".
        _cmdHelper.RunCmdAndGetOutput(default, default(string)).ReturnsForAnyArgs("1.22.22");
        services.AddTransient(provider => _cmdHelper);
    }

    [Theory]
    [InlineData("package-lock.json", "npm install --ignore-scripts")]
    [InlineData("pnpm-lock.yaml", "pnpm install --ignore-scripts")]
    [InlineData("yarn.lock", "yarn --ignore-scripts")]
    public async Task Should_Install_React_Project_With_The_Package_Manager_It_Declares(
        string lockFile, string expectedCommand)
    {
        using var project = new TemporaryJavaScriptProject(ReactPackageJson, lockFile);

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd(expectedCommand, project.Directory);
    }

    [Fact]
    public async Task Should_Install_React_Project_With_Npm_When_It_Declares_No_Package_Manager()
    {
        using var project = new TemporaryJavaScriptProject(ReactPackageJson);

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd("npm install --ignore-scripts", project.Directory);
        _cmdHelper.DidNotReceive().RunCmd("yarn --ignore-scripts", Arg.Any<string>());
    }

    [Fact]
    public async Task Should_Keep_Using_Yarn_For_Other_JavaScript_Frameworks_Without_A_Lock_File()
    {
        using var project = new TemporaryJavaScriptProject(VuePackageJson);

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd("yarn --ignore-scripts", project.Directory);
    }

    [Fact]
    public async Task Should_Keep_Using_Yarn_For_Other_JavaScript_Frameworks()
    {
        // The project has an npm lock file on purpose: only React projects follow the declared package manager.
        using var project = new TemporaryJavaScriptProject(VuePackageJson, "package-lock.json");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        _cmdHelper.Received(1).RunCmd("yarn --ignore-scripts", project.Directory);
        _cmdHelper.DidNotReceive().RunCmd("npm install --ignore-scripts", Arg.Any<string>());
    }

    [Fact]
    public async Task Should_Copy_The_Dependency_Version_Nested_Under_The_Declaring_Package()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js", "nested");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("nested");
    }

    [Fact]
    public async Task Should_Copy_The_Root_Dependency_When_The_Declaring_Package_Has_No_Nested_Copy()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("root");
    }

    [Fact]
    public async Task Should_Copy_A_Dependency_That_Exists_Only_Under_The_Declaring_Package()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery-migrate/dist/jquery-migrate.js", "migrate");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery-migrate/jquery-migrate.js").ShouldBe("migrate");
        project.ReadFile("wwwroot/libs/abp/jquery/abp.jquery.js").ShouldBe("abp");
    }

    [Fact]
    public async Task Should_Copy_From_The_Root_Package_When_The_Package_Is_Installed_More_Than_Once()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js", "root package");
        project.WriteFile("node_modules/a-package/node_modules/@abp/jquery/abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/jquery/dist/jquery.js\": \"@libs/jquery/\" } }");
        project.WriteFile("node_modules/a-package/node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js", "nested package");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("root package");
    }

    [Fact]
    public async Task Should_Copy_A_Mapping_That_Only_Another_Copy_Of_A_Package_Declares_From_The_Root()
    {
        using var project = new TemporaryJavaScriptProject("{ \"dependencies\": { \"@abp/widget\": \"1.0.0\" } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("node_modules/@abp/widget/abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/widget/dist/main.js\": \"@libs/widget/\" } }");
        project.WriteFile("node_modules/a-package/node_modules/@abp/widget/abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/widget/dist/main.css\": \"@libs/widget/\" } }");
        project.WriteFile("node_modules/widget/dist/main.js", "js");
        project.WriteFile("node_modules/widget/dist/main.css", "css");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/widget/main.js").ShouldBe("js");
        project.ReadFile("wwwroot/libs/widget/main.css").ShouldBe("css");
    }

    [Fact]
    public async Task Should_Prefer_The_Root_Copy_Of_A_Package_Whose_Mapping_File_Is_In_A_Subfolder()
    {
        const string mappingFile = "module.exports = { mappings: { \"@node_modules/dep/dist/dep.js\": \"@libs/dep/\" } }";
        using var project = new TemporaryJavaScriptProject("{ \"dependencies\": { \"@scope/widget\": \"1.0.0\" } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("node_modules/@scope/widget/config/browser/release/abp.resourcemapping.js", mappingFile);
        project.WriteFile("node_modules/@scope/widget/node_modules/dep/dist/dep.js", "root package");
        project.WriteFile("node_modules/a/node_modules/@scope/widget/abp.resourcemapping.js", mappingFile);
        project.WriteFile("node_modules/a/node_modules/@scope/widget/node_modules/dep/dist/dep.js", "nested package");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/dep/dep.js").ShouldBe("root package");
    }

    [Fact]
    public async Task Should_Copy_The_Root_Dependency_When_The_Clean_Rules_Delete_The_Nested_Copy()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("abp.resourcemapping.js",
            "module.exports = { clean: [ \"@node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js\" ], mappings: { } }");
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js", "nested");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("root");
    }

    [Fact]
    public async Task Should_Prefer_A_Scoped_Root_Package_Over_An_Unscoped_Root_Package()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("node_modules/zz-widget/abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/jquery/dist/jquery.js\": \"@libs/jquery/\" } }");
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js", "nested");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("nested");
    }

    [Fact]
    public async Task Should_Copy_The_Root_Dependency_When_The_Nested_Copy_Has_No_Matching_File()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery/src/jquery.js", "nested");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("root");
    }

    [Fact]
    public async Task Should_Copy_From_A_Package_When_A_Shallower_File_Has_No_File_For_The_Same_Mapping()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/jquery-migrate/dist/jquery-migrate.js\": \"@libs/jquery-migrate/\" } }");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery-migrate/dist/jquery-migrate.js", "migrate");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery-migrate/jquery-migrate.js").ShouldBe("migrate");
    }

    [Fact]
    public async Task Should_Copy_A_Source_To_Every_Destination_That_Maps_It()
    {
        using var project = CreateMvcProjectWithJQueryPack();
        project.WriteFile("abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js\": \"@libs/legacy/jquery/\" } }");
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js", "nested");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("nested");
        project.ReadFile("wwwroot/libs/legacy/jquery/jquery.js").ShouldBe("nested");
    }

    [Fact]
    public async Task Should_Not_Mix_The_Files_Of_A_Package_That_Is_Installed_More_Than_Once()
    {
        const string mappingFile = "module.exports = { mappings: { \"@node_modules/widget/dist/*.js\": \"@libs/widget/\" } }";
        using var project = new TemporaryJavaScriptProject("{ \"dependencies\": { \"@abp/widget\": \"1.0.0\" } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("node_modules/@abp/widget/abp.resourcemapping.js", mappingFile);
        project.WriteFile("node_modules/@abp/widget/node_modules/widget/dist/main.js", "root package");
        project.WriteFile("node_modules/a-package/node_modules/@abp/widget/abp.resourcemapping.js", mappingFile);
        project.WriteFile("node_modules/a-package/node_modules/@abp/widget/node_modules/widget/dist/main.js", "nested package");
        project.WriteFile("node_modules/a-package/node_modules/@abp/widget/node_modules/widget/dist/legacy.js", "nested package");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/widget/main.js").ShouldBe("root package");
        File.Exists(Path.Combine(project.Directory, "wwwroot/libs/widget/legacy.js")).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Resolve_A_Pattern_Written_With_Backslashes()
    {
        using var project = new TemporaryJavaScriptProject("{ \"dependencies\": { \"@abp/jquery\": \"~10.7.1\" } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("node_modules/@abp/jquery/abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules\\\\jquery-migrate\\\\dist\\\\jquery-migrate.js\": \"@libs/jquery-migrate/\" } }");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery-migrate/dist/jquery-migrate.js", "migrate");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery-migrate/jquery-migrate.js").ShouldBe("migrate");
    }

    [Fact]
    public async Task Should_Not_Copy_The_Nested_Dependencies_Of_Another_Copy_Of_A_Package()
    {
        using var project = new TemporaryJavaScriptProject("{ \"dependencies\": { \"@abp/jquery\": \"~9.3.7\" } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("node_modules/@abp/jquery/abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/jquery/dist/jquery.js\": \"@libs/jquery/\" } }");
        project.WriteFile("node_modules/jquery/dist/jquery.js", "jquery 3");
        project.WriteFile("node_modules/a-package/node_modules/@abp/jquery/abp.resourcemapping.js",
            "module.exports = { mappings: { " +
            "\"@node_modules/jquery/dist/jquery.js\": \"@libs/jquery/\", " +
            "\"@node_modules/jquery-migrate/dist/jquery-migrate.js\": \"@libs/jquery-migrate/\" } }");
        project.WriteFile("node_modules/a-package/node_modules/@abp/jquery/node_modules/jquery-migrate/dist/jquery-migrate.js", "migrate 4");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("jquery 3");
        File.Exists(Path.Combine(project.Directory, "wwwroot/libs/jquery-migrate/jquery-migrate.js")).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Resolve_The_Mappings_Of_The_Project_Against_The_Root_Node_Modules()
    {
        using var project = new TemporaryJavaScriptProject("{ \"dependencies\": { } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("ClientApp/abp.resourcemapping.js",
            "module.exports = { mappings: { \"@node_modules/widget/dist/main.js\": \"@libs/widget/\" } }");
        project.WriteFile("node_modules/widget/dist/main.js", "root");
        project.WriteFile("ClientApp/node_modules/widget/dist/main.js", "client app");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/widget/main.js").ShouldBe("root");
    }

    [Fact]
    public async Task Should_Resolve_A_Pattern_Without_The_Node_Modules_Alias()
    {
        using var project = new TemporaryJavaScriptProject("{ \"dependencies\": { \"@abp/jquery\": \"~10.7.1\" } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("node_modules/@abp/jquery/abp.resourcemapping.js",
            "module.exports = { mappings: { \"node_modules/jquery-migrate/dist/jquery-migrate.js\": \"@libs/jquery-migrate/\" } }");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery-migrate/dist/jquery-migrate.js", "migrate");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery-migrate/jquery-migrate.js").ShouldBe("migrate");
    }

    [Fact]
    public async Task Should_Copy_The_Root_Version_Of_A_Direct_Dependency_Of_The_Project()
    {
        using var project = CreateMvcProjectWithJQueryPack("\"jquery\": \"4.1.0\"");
        project.WriteFile("node_modules/jquery/dist/jquery.js", "root");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery/dist/jquery.js", "nested");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery-migrate/dist/jquery-migrate.js", "migrate");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery/jquery.js").ShouldBe("root");
        project.ReadFile("wwwroot/libs/jquery-migrate/jquery-migrate.js").ShouldBe("migrate");
    }

    [Fact]
    public async Task Should_Copy_The_Nested_Copy_Of_A_Direct_Dependency_That_Is_Not_Installed_To_The_Root()
    {
        using var project = CreateMvcProjectWithJQueryPack("\"jquery-migrate\": \"4.0.2\"");
        project.WriteFile("node_modules/@abp/jquery/node_modules/jquery-migrate/dist/jquery-migrate.js", "migrate");

        await GetRequiredService<IInstallLibsService>().InstallLibsAsync(project.Directory);

        project.ReadFile("wwwroot/libs/jquery-migrate/jquery-migrate.js").ShouldBe("migrate");
    }

    private static TemporaryJavaScriptProject CreateMvcProjectWithJQueryPack(string additionalDependencies = null)
    {
        var dependencies = "\"@abp/jquery\": \"~10.7.1\"" + (additionalDependencies == null ? string.Empty : ", " + additionalDependencies);
        var project = new TemporaryJavaScriptProject("{ \"dependencies\": { " + dependencies + " } }");
        project.WriteFile("MyProject.Web.csproj", "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        project.WriteFile("node_modules/@abp/jquery/abp.resourcemapping.js",
            "module.exports = { mappings: { " +
            "\"@node_modules/jquery/dist/jquery.js\": \"@libs/jquery/\", " +
            "\"@node_modules/jquery-migrate/dist/jquery-migrate.js\": \"@libs/jquery-migrate/\", " +
            "\"@node_modules/@abp/jquery/src/*.*\": \"@libs/abp/jquery/\" } }");
        project.WriteFile("node_modules/@abp/jquery/src/abp.jquery.js", "abp");
        return project;
    }
}
