using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.Domain.Values;

public class ValueObject_Tests
{
    [Fact]
    public void ValueObjects_With_Same_Properties_Should_Be_Equals()
    {
        var cityId = Guid.NewGuid();

        var address1 = new Address(cityId, "Baris Manco", 42, "home", "office");
        var address2 = new Address(cityId, "Baris Manco", 42, "home", "office");

        address1.ValueEquals(address2).ShouldBeTrue();
    }

    [Fact]
    public void ValueObjects_With_Different_Properties_Should_Not_Be_Equals()
    {
        var cityId = Guid.NewGuid();

        var address1 = new Address(cityId, "Baris Manco", 42);
        var address2 = new Address(cityId, "Baris Manco", 43);

        address1.ValueEquals(address2).ShouldBeFalse();

        address1 = new Address(cityId, "Baris Manco", 42, "home", "office");
        address2 = new Address(cityId, "Baris Manco", 42, "home");

        address1.ValueEquals(address2).ShouldBeFalse();

        var emailAddress1 = new EmailAddress("test@abp.io");
        var emailAddress2 = new EmailAddress(null);

        emailAddress1.ValueEquals(emailAddress2).ShouldBeFalse();
    }

    [Fact]
    public void ValueObjects_Recursively_ValueEquals()
    {
        var cityId = Guid.NewGuid();

        var address1 = new AddressWithZipCode(cityId, "Baris Manco", 42, new ZipCode("0000001"), "home", "office");
        var address2 = new AddressWithZipCode(cityId, "Baris Manco", 42, new ZipCode("0000001"), "home", "office");

        address1.ValueEquals(address2).ShouldBeTrue();

        address1 = new AddressWithZipCode(cityId, "Baris Manco", 42, new ZipCode("0000001"), "home", "office");
        address2 = new AddressWithZipCode(cityId, "Baris Manco", 42, new ZipCode("0000002"), "home", "office");

        address1.ValueEquals(address2).ShouldBeFalse();
    }
}
