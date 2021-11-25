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
        var address1 = new Address(cityId, "Baris Manco", 42);
        var address2 = new Address(cityId, "Baris Manco", 42);

        address1.ValueEquals(address2).ShouldBeTrue();
    }

    [Fact]
    public void ValueObjects_With_Different_Properties_Should_Not_Be_Equals()
    {
        var cityId = Guid.NewGuid();

        var address1 = new Address(cityId, "Baris Manco", 42);
        var address2 = new Address(cityId, "Baris Manco", 43);

        address1.ValueEquals(address2).ShouldBeFalse();
    }
}
