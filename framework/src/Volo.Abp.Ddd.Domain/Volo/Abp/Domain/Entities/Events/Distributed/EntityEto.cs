using System;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    [Serializable]
    public class EntityEto : EtoBase
    {
        public string EntityType { get; set; }

        public string KeysAsString { get; set; }

        public EntityEto()
        {

        }

        public EntityEto(string entityType, string keysAsString)
        {
            EntityType = entityType;
            KeysAsString = keysAsString;
        }
    }
}