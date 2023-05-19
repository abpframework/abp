using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;

public class EntityActionDictionary : Dictionary<string, List<EntityAction>>
{
    public List<EntityAction> Get<T>()
    {
        return this.GetOrAdd(typeof(T).FullName, () => new List<EntityAction>());
    }
}
