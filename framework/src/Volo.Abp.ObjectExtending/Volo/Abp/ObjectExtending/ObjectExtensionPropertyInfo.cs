using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionPropertyInfo
    {
        public string Name { get; }

        public List<ValidationAttribute> ValidationAttributes { get; }

        public Dictionary<object, object> Configuration { get; }

        public ObjectExtensionPropertyInfo(string name)
        {
            Name = name;
            ValidationAttributes = new List<ValidationAttribute>();
            Configuration = new Dictionary<object, object>();
        }
    }
}
