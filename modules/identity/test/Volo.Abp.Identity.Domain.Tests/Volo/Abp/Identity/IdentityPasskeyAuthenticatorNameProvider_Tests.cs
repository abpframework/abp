using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityPasskeyAuthenticatorNameProvider_Tests : AbpIdentityDomainTestBase
{
    private readonly IdentityPasskeyAuthenticatorNameProvider _nameProvider;

    public IdentityPasskeyAuthenticatorNameProvider_Tests()
    {
        _nameProvider = GetRequiredService<IdentityPasskeyAuthenticatorNameProvider>();
    }

    [Fact]
    public async Task Should_Return_Name_Of_Known_Authenticator()
    {
        var aaguid = new Guid("08987058-cadc-4b81-b6e1-30de50dcbe96").ToByteArray(bigEndian: true);

        (await _nameProvider.GetNameOrNullAsync(aaguid)).ShouldBe("Windows Hello");
    }

    [Fact]
    public async Task Should_Read_Aaguid_As_Big_Endian()
    {
        var aaguid = new Guid("08987058-cadc-4b81-b6e1-30de50dcbe96").ToByteArray();

        (await _nameProvider.GetNameOrNullAsync(aaguid)).ShouldBeNull();
    }

    [Fact]
    public async Task Should_Return_Null_For_Unknown_Or_Invalid_Aaguid()
    {
        (await _nameProvider.GetNameOrNullAsync(null)).ShouldBeNull();
        (await _nameProvider.GetNameOrNullAsync(new byte[16])).ShouldBeNull();
        (await _nameProvider.GetNameOrNullAsync(new byte[] { 1, 2, 3 })).ShouldBeNull();
        (await _nameProvider.GetNameOrNullAsync(Guid.NewGuid().ToByteArray(bigEndian: true))).ShouldBeNull();
    }
}
