using System.Collections.Generic;
using Shouldly;
using Volo.Abp.Json;
using Xunit;

namespace Volo.Abp.Cli.ProjectModification;

/// <summary>
/// abp.io sends the application type of an NPM package as a number.
/// These tests read the responses the same way the CLI does (through <see cref="IJsonSerializer"/>).
/// </summary>
public class NpmPackageInfo_Tests : AbpCliTestBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public NpmPackageInfo_Tests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    [Theory]
    [InlineData(1, NpmApplicationType.Mvc)]
    [InlineData(2, NpmApplicationType.Angular)]
    [InlineData(4, NpmApplicationType.React)]
    public void Should_Deserialize_ApplicationType_From_Its_Number(int value, NpmApplicationType expected)
    {
        // Shape of "api/app/npmPackage/byName"
        var json = "{\"id\":\"3a1f0b9e-6f0b-4a51-9f39-3d2a6f1b7c10\",\"name\":\"@volo/abp-react-app-config\"," +
                   "\"description\":null,\"applicationType\":" + value + ",\"isPro\":true," +
                   "\"minVersion\":null,\"maxVersion\":null}";

        var package = _jsonSerializer.Deserialize<NpmPackageInfo>(json);

        package.Name.ShouldBe("@volo/abp-react-app-config");
        package.ApplicationType.ShouldBe(expected);
    }

    [Fact]
    public void Should_Deserialize_React_Package_In_Downloadable_Package_List()
    {
        // Shape of "api/download/npmPackages"
        const string json = "[" +
                            "{\"name\":\"@abp/ng.core\",\"applicationType\":2,\"minVersion\":null,\"maxVersion\":null}," +
                            "{\"name\":\"@volo/abp-react-oidc-auth\",\"applicationType\":4,\"minVersion\":null,\"maxVersion\":null}" +
                            "]";

        var packages = _jsonSerializer.Deserialize<List<NpmPackageInfo>>(json);

        packages.Count.ShouldBe(2);
        packages[0].ApplicationType.ShouldBe(NpmApplicationType.Angular);
        packages[1].ApplicationType.ShouldBe(NpmApplicationType.React);
    }

    [Fact]
    public void Should_Not_Fail_For_An_ApplicationType_This_Cli_Version_Does_Not_Know()
    {
        // A CLI that is older than abp.io receives values it has no enum member for.
        // It must keep working and simply not treat the package as one of the types it knows.
        const string json = "{\"name\":\"@volo/abp-some-future-ui\",\"applicationType\":64}";

        var package = _jsonSerializer.Deserialize<NpmPackageInfo>(json);

        package.Name.ShouldBe("@volo/abp-some-future-ui");
        ((int)package.ApplicationType).ShouldBe(64);
        package.ApplicationType.HasFlag(NpmApplicationType.Mvc).ShouldBeFalse();
        package.ApplicationType.HasFlag(NpmApplicationType.Angular).ShouldBeFalse();
        package.ApplicationType.HasFlag(NpmApplicationType.React).ShouldBeFalse();
    }
}
