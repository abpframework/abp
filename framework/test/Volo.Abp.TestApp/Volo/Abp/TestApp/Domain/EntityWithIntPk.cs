using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain;

public class EntityWithIntPk : AggregateRoot<int>
{
    public string Name { get; set; }

    public EntityWithIntPk()
    {

    }

    public EntityWithIntPk(string name)
    {
        Name = name;
    }
}
