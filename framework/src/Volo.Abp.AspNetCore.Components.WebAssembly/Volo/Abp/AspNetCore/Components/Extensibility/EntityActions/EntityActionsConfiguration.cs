using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Components.Extensibility.EntityActions
{
    public class EntityActionsConfiguration
    {
        protected EntityActionDictionary EntityActions { get; set; }

        public EntityActionsConfiguration()
        {
            EntityActions = new EntityActionDictionary();
        }

        public List<EntityAction> Get<T>()
        {
            return EntityActions.GetOrAdd(typeof(T).FullName, () => new List<EntityAction>());
        }
    }
}
