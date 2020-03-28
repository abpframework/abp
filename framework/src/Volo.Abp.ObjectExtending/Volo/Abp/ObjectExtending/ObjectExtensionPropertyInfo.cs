using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionPropertyInfo
    {
        [NotNull]
        public ObjectExtensionInfo ObjectExtension { get; }

        [NotNull]
        public string Name { get; }

        //[NotNull] //TODO: Will be implemented, probably in the v2.5
        //public List<ValidationAttribute> ValidationAttributes { get; }

        [NotNull]
        public Dictionary<object, object> Configuration { get; }

        public ObjectExtensionPropertyInfo(ObjectExtensionInfo objectExtension, string name)
        {
            ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
            Name = Check.NotNull(name, nameof(name));

            //ValidationAttributes = new List<ValidationAttribute>();
            Configuration = new Dictionary<object, object>();
        }
    }
}
