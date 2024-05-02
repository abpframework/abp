using System;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Values;

public class Address : ValueObject
{
    public Guid CityId { get; }

    public string Street { get; }

    public int Number { get; }

    public string[] Tags { get; }

    private Address()
    {
    }

    public Address(
        Guid cityId,
        string street,
        int number,
        params string[] tags)
    {
        CityId = cityId;
        Street = street;
        Number = number;
        Tags = tags;
    }

    //Requires to implement this method to return properties.
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return CityId;
        yield return Number;
        foreach (var tag in Tags)
        {
            yield return tag;
        }
    }
}
