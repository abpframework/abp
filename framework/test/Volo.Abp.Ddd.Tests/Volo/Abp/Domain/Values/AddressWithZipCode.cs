using System;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Values;

public class AddressWithZipCode : ValueObject
{
    public Guid CityId { get; }

    public string Street { get; }

    public int Number { get; }

    public ZipCode ZipCode { get; }

    public string[] Tags { get; }

    private AddressWithZipCode()
    {
    }

    public AddressWithZipCode(
        Guid cityId,
        string street,
        int number,
        ZipCode zipCode,
        params string[] tags)
    {
        CityId = cityId;
        Street = street;
        Number = number;
        ZipCode = zipCode;
        Tags = tags;
    }

    //Requires to implement this method to return properties.
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return CityId;
        yield return Number;
        yield return ZipCode;
        foreach (var tag in Tags)
        {
            yield return tag;
        }
    }
}

public class ZipCode : ValueObject
{
    public string Code { get; }

    public ZipCode(string code)
    {
        Code = code;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
    }
}
