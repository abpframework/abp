using System.IO;
using Shouldly;
using Volo.Abp.Cli.ProjectModification;
using Xunit;

namespace Volo.Abp.Cli;

public class ProjectFinder_Tests
{
    [Fact]
    public void FindNpmTargetProjectFile_Tests()
    {
        var npmTargets = ProjectFinder.FindNpmTargetProjectFile(
            new[]
            {
                    GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.Host.csproj"),
                    GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.Web.Host.csproj"),
                    GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.HttpApi.Host.csproj"),
                    GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.Application.Contracts.csproj"),
                    GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.Application.csproj"),
                    GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.EntityFrameworkCore.csproj")
            }
        );

        npmTargets.ShouldContain(GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.Host.csproj"));
        npmTargets.ShouldContain(GetNormalizedPath(@"c:\temp\project\folder\src\Acme.PhoneBook.Web.Host.csproj"));
    }

    [Fact]
    public void GetAssemblyName_Tests()
    {
        var assemblyName = ProjectFileNameHelper.GetAssemblyNameFromProjectPath(
            GetNormalizedPath(
                @"c:\temp\project\folder\src\Acme.PhoneBook.Host.csproj"
            )
        );

        assemblyName.ShouldBe("Acme.PhoneBook.Host");
    }

    private static string GetNormalizedPath(string path)
    {
        return path.Replace('\\', Path.DirectorySeparatorChar);
    }
}
