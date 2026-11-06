using System.Collections.Generic;
using Shouldly;
using Volo.Abp.Cli.ProjectModification;
using Xunit;

namespace Volo.Abp.Cli;

public class MyGetApiResponse_Tests
{
    [Fact]
    public void GetLatestVersionOrNull_Should_Return_Last_Version_Of_Package()
    {
        var response = new MyGetApiResponse
        {
            Packages = new List<MyGetPackage>
            {
                new() { Id = "Volo.Abp.Account", Versions = new List<string> { "10.6.0", "10.7.0-preview.1" } },
                new() { Id = "Volo.Abp.Core", Versions = new List<string> { "10.6.0", "10.7.0-preview.1", "10.7.0-preview.2" } }
            }
        };

        response.GetLatestVersionOrNull("Volo.Abp.Core").ShouldBe("10.7.0-preview.2");
    }

    [Fact]
    public void GetLatestVersionOrNull_Should_Return_Null_When_Package_Does_Not_Exist()
    {
        var response = new MyGetApiResponse
        {
            Packages = new List<MyGetPackage>
            {
                new() { Id = "Volo.Abp.Account", Versions = new List<string> { "10.7.0-preview.1" } }
            }
        };

        response.GetLatestVersionOrNull("Volo.Abp.Core").ShouldBeNull();
    }

    [Fact]
    public void GetLatestVersionOrNull_Should_Return_Null_When_Packages_Is_Null()
    {
        new MyGetApiResponse { Packages = null }.GetLatestVersionOrNull("Volo.Abp.Core").ShouldBeNull();
    }

    [Fact]
    public void GetLatestVersionOrNull_Should_Return_Null_When_Package_Versions_Is_Null()
    {
        var response = new MyGetApiResponse
        {
            Packages = new List<MyGetPackage>
            {
                new() { Id = "Volo.Abp.Core", Versions = null }
            }
        };

        response.GetLatestVersionOrNull("Volo.Abp.Core").ShouldBeNull();
    }

    [Fact]
    public void GetLatestVersionOrNull_Should_Ignore_Null_Package_Entries()
    {
        var response = new MyGetApiResponse
        {
            Packages = new List<MyGetPackage>
            {
                null,
                new() { Id = "Volo.Abp.Core", Versions = new List<string> { "10.7.0-preview.1" } }
            }
        };

        response.GetLatestVersionOrNull("Volo.Abp.Core").ShouldBe("10.7.0-preview.1");
    }
}
