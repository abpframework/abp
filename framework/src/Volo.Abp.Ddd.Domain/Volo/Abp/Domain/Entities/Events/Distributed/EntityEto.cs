using System;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    [Serializable]
    public class EntityEto : EtoBase
    {
        public string KeysAsString { get; set; }

        public EntityEto()
        {

        }

        public EntityEto(string keysAsString)
        {
            KeysAsString = keysAsString;
        }
    }
}