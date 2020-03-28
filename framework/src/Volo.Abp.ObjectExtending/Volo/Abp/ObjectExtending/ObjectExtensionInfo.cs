using System.Collections.Generic;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionInfo
    {
        public Dictionary<string, ObjectExtensionPropertyInfo> Properties { get; }

        public Dictionary<object, object> Configuration { get; }

        public ObjectExtensionInfo()
        {
            Properties = new Dictionary<string, ObjectExtensionPropertyInfo>();
            Configuration = new Dictionary<object, object>();
        }
    }
}