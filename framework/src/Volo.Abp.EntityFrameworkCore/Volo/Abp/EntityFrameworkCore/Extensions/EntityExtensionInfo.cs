using System.Collections.Generic;

namespace Volo.Abp.EntityFrameworkCore.Extensions
{
    public class EntityExtensionInfo
    {
        public Dictionary<string, PropertyExtensionInfo> Properties { get; set; }

        public EntityExtensionInfo()
        {
            Properties = new Dictionary<string, PropertyExtensionInfo>();
        }
    }
}