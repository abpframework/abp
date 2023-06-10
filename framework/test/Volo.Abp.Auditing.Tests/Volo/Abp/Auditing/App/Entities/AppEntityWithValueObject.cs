using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace Volo.Abp.Auditing.App.Entities;

public class AppEntityWithValueObject : AggregateRoot<Guid>
{
    protected AppEntityWithValueObject()
    {

    }

    public AppEntityWithValueObject(Guid id, string name, AppEntityWithValueObjectAddress appEntityWithValueObjectAddress)
        : base(id)
    {
        Name = name;
        AppEntityWithValueObjectAddress = appEntityWithValueObjectAddress;
    }

    public string Name { get; set; }

    public AppEntityWithValueObjectAddress AppEntityWithValueObjectAddress { get; set; }
}

public class AppEntityWithValueObjectAddress : ValueObject
{
    public AppEntityWithValueObjectAddress(string country)
    {

        Country = country;
    }

    public string Country { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Country;
    }
}
